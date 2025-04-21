using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthorization(opts => {
//  
//     //политики для cookies
//     opts.AddPolicy("OnlyForLondon", policy => {
//         policy.RequireClaim(ClaimTypes.Locality, "Лондон", "London")
//             .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
//     });
//     opts.AddPolicy("OnlyForMicrosoft", policy => {
//         policy.RequireClaim("company", "Microsoft");
//     });
//     
//     // Политика только для JWT
//     opts.AddPolicy("api", policy => {
//         policy.RequireClaim("api_access", "Yes")
//             .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
//     });
// });

builder.Services.AddAuthorization(opts => {
    // Политики для Web
    opts.AddPolicy("OnlyForLondon", policy => {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim(ClaimTypes.Locality, "Лондон", "London");
    });
    
    opts.AddPolicy("OnlyForMicrosoft", policy => {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim("company", "Microsoft");
    });
    
    // Политика для Api
    opts.AddPolicy("api_access", policy => {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireClaim("api_access", "Yes");
    });
    
    // Политика для Web + api
    opts.AddPolicy("api_web_access", policy =>
    {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                var hasClaim = context.User.HasClaim("api_access", "Yes") ||
                               context.User.HasClaim("company", "Microsoft");
                return hasClaim;
            });
    });
});


// Настройка аутентификации (оба типа)
builder.Services.AddAuthentication(options =>
    {
        // Указываем схемы по умолчанию (порядок важен!)
        options.DefaultScheme = "JWT_OR_COOKIE";
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    // Добавляем cookie-аутентификацию
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
    })
    // Добавляем JWT-аутентификацию
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
        };
    })
    //Добавляем политику, которая проверяет обе схемы
      .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            // Если есть заголовок Authorization с Bearer токеном - используем JWT
            string authorization = context.Request.Headers.Authorization;
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                return JwtBearerDefaults.AuthenticationScheme;
            
            // Иначе используем cookie
            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<UsersDb>();

var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 


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

app.Map("/", [Authorize](HttpContext context) =>
{
    var login = context.User.FindFirst(ClaimTypes.Name);
    var city = context.User.FindFirst(ClaimTypes.Locality);
    var company = context.User.FindFirst("company");
    return $"Name: {login?.Value}\nCity: {city?.Value}\nCompany: {company?.Value}";
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

// web+api доступ
app.MapGet("/hello_api_jwt", [Authorize("api_web_access")]() => "web + api Hello");

// web "OnlyForLondon"
app.Map("/london", [Authorize("OnlyForLondon")]() => "You are living in London");

// web "OnlyForMicrosoft"
app.Map("/microsoft", [Authorize("OnlyForMicrosoft")]() => "You are working in Microsoft");

// api доступ
app.MapGet("/hello_jwt", [Authorize("api_access")]() => "api Hello");

app.Run();

