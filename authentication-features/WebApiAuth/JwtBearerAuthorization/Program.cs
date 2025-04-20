using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();  
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			// указывает, будет ли валидироваться издатель при валидации токена
			ValidateIssuer = true,
			// строка, представляющая издателя
			ValidIssuer = AuthOptions.ISSUER,
			// будет ли валидироваться потребитель токена
			ValidateAudience = true,
			// установка потребителя токена
			ValidAudience = AuthOptions.AUDIENCE,
			// будет ли валидироваться время существования
			ValidateLifetime = true,
			// установка ключа безопасности
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			// валидация ключа безопасности
			ValidateIssuerSigningKey = true,
		};
	});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<UsersDb>();

var app = builder.Build();


app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 


//Генерация токена
app.MapPost("/login", (Person loginData, [FromServices]UsersDb userDb) => 
{
	// находим пользователя 
	Person? person = userDb.GetAuthPerson(loginData.Email, loginData.Password);
	// если пользователь не найден, отправляем статусный код 401
	if(person is null) return Results.Unauthorized();
	
	
	var claims = new List<Claim> {new Claim(ClaimTypes.Name, person.Email) };
	// создаем JWT-токен
	var jwt = new JwtSecurityToken(
		issuer: AuthOptions.ISSUER,
		audience: AuthOptions.AUDIENCE,
		claims: claims,
		expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(200)),
		signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
	var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
	
	var response = new
	{
		access_token = encodedJwt,
		username = person.Email
	};
 
	return Results.Json(response);
});


app.Map("/hello", [Authorize]() => "Hello World!");

app.Run();

