namespace Hangfire_InPgSqlStorage.Jobs;

public class SimpleJob
{
    private readonly ILogger<SimpleJob> _logger;

    public SimpleJob(ILogger<SimpleJob> logger)
    {
        _logger = logger;
        _logger.LogInformation("ctor SimpleJob exec");
    }


    public void DoSomething()
    {
        _logger.LogInformation(" DoSomething SimpleJob exec");
    }
}