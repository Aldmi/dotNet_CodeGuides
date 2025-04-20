using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Настройка политик авторизации
builder.Services.AddAuthorization(opts => {
    // Общие политики (могут использовать обе схемы)
    opts.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
    
    // Политики для cookies
    opts.AddPolicy("OnlyForLondon", policy => {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim(ClaimTypes.Locality, "Лондон", "London");
    });
    
    opts.AddPolicy("OnlyForMicrosoft", policy => {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim("company", "Microsoft");
    });
    
    // Политика только для JWT
    opts.AddPolicy("api", policy => {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireClaim("api_access", "Yes");
    });
});


// Настройка аутентификации
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            // Важно для .NET 8:
            ClockSkew = TimeSpan.Zero // Убираем запас времени для expiration
        };
        
        // Для отладки
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<UsersDb>();

var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 


//cookies -------------------------------------------------------------------------
app.MapGet("/login", async (HttpContext context) =>
{
	context.Response.ContentType = "text/html; charset=utf-8";
	// html-форма для ввода логина/пароля
	string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Email</label><br />
                <input name='email' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
	await context.Response.WriteAsync(loginForm);
});


app.MapPost("/login", async (string? returnUrl, HttpContext context, [FromServices]UsersDb userDb) =>
{
    // получаем из формы email и пароль
    var form = context.Request.Form;
    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email и/или пароль не установлены");
 
    string email = form["email"];
    string password = form["password"];
    Person? person = userDb.GetAuthPerson(email, password);
    if (person is null) return Results.Unauthorized();
 
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, person.Email),
        new Claim(ClaimTypes.Locality, person.City),
        new Claim("company", person.Company)
    };
    // создаем объект ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // будут формироваться аутентификационные куки, которые будут отправлены клиенту 
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl ?? "/");
});


app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});




//jwt generation-------------------------------------------------------------------------
//Генерация токена
app.MapPost("/jwt", ( [FromServices]UsersDb userDb) => 
{
    var claims = new List<Claim> {new Claim("api_access", "Yes") };
    // создаем JWT-токен
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromDays(200)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
    var response = new
    {
        access_token = encodedJwt,
    };
    return Results.Json(response);
});


//endpoints---------------------------------------------------------------------------

app.Map("/", [Authorize](HttpContext context) =>
{
    var login = context.User.FindFirst(ClaimTypes.Name);
    var city = context.User.FindFirst(ClaimTypes.Locality);
    var company = context.User.FindFirst("company");
    return $"Name: {login?.Value}\nCity: {city?.Value}\nCompany: {company?.Value}";
});

// доступ только для City = London
app.Map("/london", [Authorize("OnlyForLondon")]() => "You are living in London");

// доступ только для Company = Microsoft
app.Map("/microsoft", [Authorize(Policy = "OnlyForMicrosoft")]() => "You are working in Microsoft");

// api доступ
app.MapGet("/hello_jwt", [Authorize]() => "Hello World!");


app.Run();



