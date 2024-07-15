using LongRunning.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LongRunning.Api.Jobs;

public class LongRunningJobWithNotification(ILogger<LongRunningJob> logger, IHubContext<NotificationHub> hubContext)
{
   public async Task ExecuteAsync(CancellationToken ct)
   {
      logger.LogInformation("Starting background job");
      await Task.Delay(TimeSpan.FromSeconds(3), ct);
      logger.LogInformation("Completed background job");
      
      //Отправляем нотификацию на каждое событие, для подключенного к SignalR хабу клиента.  (Clients.All - сделанно для простоты)
      await hubContext.Clients.All.SendAsync("ReciveNotification", $"Completed processing job", ct);
   }
}