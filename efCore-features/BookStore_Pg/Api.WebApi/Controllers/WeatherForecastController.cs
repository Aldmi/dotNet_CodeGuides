using Application.Core.Abstract;
using Application.Core.BookFeatures.Command.CreateBook;
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
    private readonly CreateBookService _createBookService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IBookContext bookContext,
        CreateBookService createBookService,
        ILogger<WeatherForecastController> logger)
    {
        _bookContext = bookContext;
        _createBookService = createBookService;
        _logger = logger;
        
        _bookContext.EnsureDeleted();
        _bookContext.EnsureCreated();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {

        await _createBookService.CreateTestBook();
        
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}