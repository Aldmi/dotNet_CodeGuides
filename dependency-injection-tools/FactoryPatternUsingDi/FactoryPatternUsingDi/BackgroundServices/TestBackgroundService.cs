using FactoryPatternUsingDi.Factories;
using FactoryPatternUsingDi.Samples;

namespace FactoryPatternUsingDi.BackgroundServices;

public class TestBackgroundService : BackgroundService
{
    private readonly ILogger<TestBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    
    /// <summary>
    /// В Singletone используем всегда IServiceScopeFactory.
    /// </summary>
    public TestBackgroundService(
        ILogger<TestBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("TestBackgroundService START");
        await DoWorkAsync(stoppingToken);
    }
    
    
    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        // using var scope = _serviceScopeFactory.CreateScope();
        // var factory = scope.ServiceProvider.GetRequiredService<IAbstractFactory<ISample1>>();
        // for (int i = 0; i < 10; i++)
        // {
        //     var obj= factory.Create(); //Создаем объекты в scope, и при выходе из функции DoWorkAsync будет вызван Dispose
        //     await Task.Delay(200, stoppingToken);
        // }
    }
}