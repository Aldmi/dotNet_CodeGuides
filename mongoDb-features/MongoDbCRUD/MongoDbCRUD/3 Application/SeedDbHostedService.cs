namespace MongoDbCRUD._3_Application;

public class SeedDbHostedService: IHostedService
{
    private readonly CarBuyingService _carBuyingService;

    public SeedDbHostedService(CarBuyingService carBuyingService)
    {
        _carBuyingService = carBuyingService;
    }
    
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var allCars = await _carBuyingService.GetAllCarsInMarket();
        if (!allCars.Any())
        {
            var adedIds = await _carBuyingService.AddManyCarsInParallelTasks(5, TimeSpan.FromSeconds(0), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}