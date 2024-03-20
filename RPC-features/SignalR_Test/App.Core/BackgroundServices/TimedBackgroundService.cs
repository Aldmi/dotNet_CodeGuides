using Infrastructure.SignalRhub;
using Infrastructure.SignalRhub.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.Core.BackgroundServices;

public class TimedBackgroundService : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<TimedBackgroundService> _logger;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));


    public TimedBackgroundService(
        IHubContext<ChatHub> hubContext,
        ILogger<TimedBackgroundService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested &&
               await _timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWorkAsync();    
        }
    }


    private async Task DoWorkAsync()
    {
        //await _hubContext.Clients.All.SendAsync("Receive", "Alex", $"Message - {DateTime.Now.ToLongTimeString()}");
        await _hubContext.Clients.All.SendAsync("Receive", new Persone("Alex", 20, DateTime.Now));
        _logger.LogInformation("Raise TimedBackgroundService");
    }
}