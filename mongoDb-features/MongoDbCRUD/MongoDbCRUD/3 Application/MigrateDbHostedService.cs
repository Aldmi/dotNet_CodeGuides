using MongoDbCRUD._2_Persistence.Interfaces;

namespace MongoDbCRUD._3_Application;

public class MigrateDbHostedService: IHostedService
{
    private readonly IDbMigrationService _migrationService;
    private readonly ILogger<MigrateDbHostedService> _logger;

    public MigrateDbHostedService(IDbMigrationService migrationService, ILogger<MigrateDbHostedService> logger)
    {
        _migrationService = migrationService;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var res=_migrationService.RunMigrate();
        if (res == null)
        {
            _logger.LogInformation("Миграции не найдены");
        }
        else
        if (res.Success)
        {
            _logger.LogInformation("Миграция прошла успешно {MirgationResult}", res.ToString());
        }
        else
        {
            _logger.LogError("Миграция закончилась ошибкой");
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}