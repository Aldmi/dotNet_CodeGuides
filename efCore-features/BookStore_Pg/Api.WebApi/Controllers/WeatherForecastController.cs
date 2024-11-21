using Application.Core.Abstract;
using Infrastructure.Persistance.Pg;
using Microsoft.AspNetCore.Mvc;

namespace Api.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IBookContext _bookContext;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IBookContext bookContext, ILogger<WeatherForecastController> logger)
    {
        _bookContext = bookContext;
        _logger = logger;
        
        // bookContext.Database.EnsureDeleted();
        // bookContext.Database.EnsureCreated();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}