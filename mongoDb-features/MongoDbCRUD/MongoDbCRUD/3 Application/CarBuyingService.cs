using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;


namespace MongoDbCRUD._3_Application;

public class CarBuyingService
{
    private readonly ICarRepository _carRepository;
    private readonly ILogger<CarBuyingService>? _logger;

    public CarBuyingService(
        ICarRepository carRepository,
        ILogger<CarBuyingService>? logger = null)
    {
        _carRepository = carRepository;
        _logger = logger;
    }

    
    public async Task<List<Car>> GetAllCarsInMarket()
    {
        return await _carRepository.Get(_=>true);
    }


    /// <summary>
    /// Добавляем машины в БД в паралельных потоках.
    /// </summary>
    /// <param name="taskCount">кол-во потоков</param>
    /// <param name="delayTime"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<Guid[]> AddManyCarsInParallelTasks(int taskCount, TimeSpan delayTime, CancellationToken ct = default)
    {
        var tasks= Enumerable.Range(1, taskCount).Select(index =>
        {
            var task = Task.Run(async () => {
                var car = CarFactory(index);
               // var id=await _carRepository.AddOrReplace(car);
                var id = await _carRepository.AddOrReplace(car);
                await Task.Delay(delayTime, ct);
                //_logger?.LogInformation($"CarId= {id!.Value}.taskIndex= {index}."); 
                return id!.Value;
            }, ct);
            return task;
        }).ToList();

        var allAddedId=await Task.WhenAll(tasks);
        return allAddedId;
    }


    public Car CarFactory(int i)
    {
        return new(
            $"Car {i}",
            DateTime.Now,
            new Customer($"Customer {i}", $"address {i}"),
            new List<ServiceCenter>
            {
                new($"ServiceCenter {i + 1}"),
                new($"ServiceCenter {i + 1}"),
                new($"ServiceCenter {i + 1}"),
            }
        );
    }
    
}