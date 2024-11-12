using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._3_Application;

namespace MongoDbCRUD.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly CarBuyingService _carBuyingService;
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMongoDatabase _mongoDatabase;

    public WeatherForecastController(CarBuyingService carBuyingService, ILogger<WeatherForecastController> logger, IMongoDatabase mongoDatabase)
    {
        _carBuyingService = carBuyingService;
        _logger = logger;
        _mongoDatabase = mongoDatabase;
    }
    

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<Car>> Get()
    {
        //var car=_carBuyingService.BuyCar("Vasy", "Lada");
        
        // var stopwatch = new Stopwatch();
        // try
        // {
        //     stopwatch.Start();
        //     var adedIds = await _carBuyingService.AddManyCarsInParallelTasks(10, TimeSpan.FromSeconds(0));
        //     stopwatch.Stop();
        //     _logger.LogWarning($"{stopwatch.Elapsed:g} {adedIds.Length}");
        // }
        // catch (Exception e)
        // {
        //     _logger.LogCritical($"Ошибка AddManyCarsInParallelTasks: '{e}'");
        // }
      
        
        var allCars = await _carBuyingService.GetAllCarsInMarket();
        
        
        // try
        //  {
        //      //string connectionString = "mongodb://root:pass12345@localhost:27017";
        //      string connectionString = "mongodb://root:pass@localhost:27017";
        //      MongoClient client = new MongoClient(connectionString);
        //      IMongoDatabase database = client.GetDatabase("food");
        //
        //      //вывести список БД
        //      using var cursor = await client.ListDatabasesAsync();
        //      var databaseDocuments = await cursor.ToListAsync();
        //      foreach (var databaseDocument in databaseDocuments)
        //      {
        //          Console.WriteLine(databaseDocument["name"]);
        //      }
        //  }
        //  catch (Exception e)
        //  {
        //      Console.WriteLine(e);
        //  }

        return allCars;
    }
}