using Domain.Core.Models;
using Infrastructure.SignalRhub.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.Core.BackgroundServices;

public class TimedBackgroundService : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<TimedBackgroundService> _logger;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(3));


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
            await SendDeviceStatisticAsync();    
            //await SendPersoneAsync();
        }
    }


    private async Task SendPersoneAsync()
    {
        await _hubContext.Clients.All.SendAsync("Receive", "Alex", $"Message - {DateTime.Now.ToLongTimeString()}");
        //await _hubContext.Clients.All.SendAsync("Receive", new Persone("Alex", 20, DateTime.Now));
        _logger.LogInformation("Raise TimedBackgroundService");
    }
    
    private async Task SendDeviceStatisticAsync()
    {
        var deviceStatisticDto = new DeviceStatistic("Device_1", "Device_1 TcpIp=50000 Addr=8", true, true, false, true, 
            new ResponsePieceOfDataVm(999, true, new EvaluateVm(1,1,1), "Send", null)
            );
        await _hubContext.Clients.All.SendAsync("Receive_DeviceStatistic", deviceStatisticDto);
        //_logger.LogInformation("Raise TimedBackgroundService SendDeviceStatisticAsync");
    }
}