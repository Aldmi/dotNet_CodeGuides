using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(opts => {
 
    opts.AddPolicy("OnlyForLondon", policy => {
        policy.RequireClaim(ClaimTypes.Locality, "Лондон", "London");
    });
    opts.AddPolicy("OnlyForMicrosoft", policy => {
        policy.RequireClaim("company", "Microsoft");
    });
});

// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/login";       // Куда перенаправлять неавторизованных
        //options.AccessDeniedPath = "/login";  // Если доступ запрещён (HTTP 403)
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Время жизни cookie
        options.SlidingExpiration = true;   // Обновлять срок при каждом запросе
        options.Cookie.HttpOnly = true;     // Защита от XSS (недоступно из JS)
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Только HTTPS
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


// доступ только для City = London
app.Map("/london", [Authorize("OnlyForLondon")]() => "You are living in London");

// доступ только для Company = Microsoft
app.Map("/microsoft", [Authorize(Policy = "OnlyForMicrosoft")]() => "You are working in Microsoft");


app.Run();

