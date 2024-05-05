using System.Security.Claims;
using IdentityDb.Pg;
using IdentityDb.Pg.Entity;
using IdentityDb.Pg.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

    //Для работы с JWT токеном
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

//Можно использовать или Cookie или BearerToken авторизацию
builder.Services.AddAuthentication()
    //.AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddApiEndpoints();

builder.Services.AddIdentityPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.ApplyMigration();
}

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.MapGet("user/me", async (ClaimsPrincipal ClaimsPrincipal, IdentityDbContext identityDbContext) =>
{
    string userId = ClaimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    var user = await identityDbContext.Users.FindAsync(userId);
    return user;
})
.RequireAuthorization();


app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                "Freezing"
            ))
        .ToArray();
    return forecast;
});

app.Run();

namespace WebApi
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
    }
}