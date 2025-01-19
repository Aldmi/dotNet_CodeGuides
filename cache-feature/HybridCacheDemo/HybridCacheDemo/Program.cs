using HybridCacheDemo;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<WeatherService>();


builder.Services.AddStackExchangeRedisCache(options =>
{
 options.Configuration = "localhost:6379";
});
builder.Services.AddHybridCache();

var app = builder.Build();

app.MapGet("/weather/{city}", async (string city, [FromServices] WeatherService weatherService) =>
	{
		var weather = await weatherService.GetCurrentWeatherHybridCacheAsync(city);
		return weather is null ? Results.NotFound() : Results.Ok(weather);
	});

app.Run();
