namespace LongRunning.Api.Jobs;

public class LongRunningJob(ILogger<LongRunningJob> logger)
{
   public async Task ExecuteAsync(CancellationToken ct)
   {
      logger.LogInformation("Starting background job");
      await Task.Delay(TimeSpan.FromSeconds(3), ct);
      logger.LogInformation("Completed background job");
   }
}