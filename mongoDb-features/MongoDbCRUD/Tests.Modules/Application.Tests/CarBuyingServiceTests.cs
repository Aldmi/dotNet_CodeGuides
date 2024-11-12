using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._3_Application;
using MongoDbCRUD._4_Persistence.MogoDb;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Modules.Application.Tests;

public class CarBuyingServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly MongoClient _client;
    const string ConnectionString = "mongodb://root:pass12345@localhost:27017";
    const string DbName = "CarsMarketDb_Parallel_Test";

    public CarBuyingServiceTests(ITestOutputHelper output)
    {
        _output = output;
        var url = MongoUrl.Create(ConnectionString);
        _client= new MongoClient(new MongoClientSettings
        {
            Server = url.Server,
            Credential = url.GetCredential(), //user, password
            MinConnectionPoolSize = 100,
            MaxConnectionPoolSize = 900,
            // WaitQueueTimeout = TimeSpan.FromSeconds(100),
            // ConnectTimeout = TimeSpan.FromSeconds(10),
        });
        _client.DropDatabase(DbName);
    }
    
    /// <summary>
    /// без использования ConnectionThrottlingPipeline, выдаеься исключение MongoDB.Driver.MongoWaitQueueFullException.
    /// Т.к. много потоков расходуют весь пулл доступных подключений к БД.
    /// ConnectionThrottlingPipeline использует Semaphore для ограничения одновременного доступа к методу работы с БД
    /// </summary>
    [Fact]
    public async Task AddManyCarsInParallelTasks_Singleton_MongoCollection()
    {
        var database = _client.GetDatabase(DbName);
        var cars = database.GetCollection<Car>(CarRepository.CollectionName);
        var connectionThrottlingPipeline = new ConnectionThrottlingPipeline(_client.Settings.MaxConnectionPoolSize);
        var carRep = new CarRepository(cars, connectionThrottlingPipeline);
        var service = new CarBuyingService(carRep);
        var tasksCount = 1000;
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var adedIds= await service.AddManyCarsInParallelTasks(tasksCount, TimeSpan.FromSeconds(0));
            adedIds.Should().HaveCount(tasksCount);
            stopwatch.Stop();
            var elapsedTime = $"{stopwatch.Elapsed:g} ";
        }
        catch (Exception e)
        {
            _output.WriteLine("EXCEPTION {0}", e);
        }
        
        var all= await carRep.Get(_ => true);
        all.Should().HaveCount(tasksCount);
        _output.WriteLine("SUCCESS !!!!  COUNT {0}", all.Count);
    }
    
    
    
    /// <summary>
    // /// Создавние новых database не помогант устранить MongoDB.Driver.MongoWaitQueueFullException
    // /// </summary>
    // [Fact]
    // public async Task AddManyCarsInParallelTasks_Many_MongoCollection()
    // {
    //     List<Guid> Sum = new List<Guid>();
    //     try
    //     {
    //         var tasksCount = 200;
    //         Parallel.For(1, 11,  i =>
    //         {                
    //             var database = _client.GetDatabase(DbName);
    //             var cars = database.GetCollection<Car>(CarRepository.CollectionName);
    //             var carRep = new CarRepository(cars);
    //             var service = new CarBuyingService(carRep);
    //             var adedIds= service.AddManyCarsInParallelTasks(tasksCount, TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
    //             Sum.AddRange(adedIds);
    //         } );
    //     }
    //     catch (Exception e)
    //     {
    //         _output.WriteLine("EXCEPTION {0}", e);
    //     }
    //     
    //     _output.WriteLine("COUNT {0}", Sum.Count);
    // }
}