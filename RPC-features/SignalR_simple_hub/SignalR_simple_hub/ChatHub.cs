using Microsoft.AspNetCore.SignalR;

namespace SignalR_simple_hub;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }
    
    
    public async Task Send(string message)
    {
       // await Clients.All.SendAsync("Receive", message);
       Console.WriteLine(message);
    }
    
    
    public override async Task OnConnectedAsync()
    {
        _logger.LogWarning("{ContextConnectionId} вошел в чат", Context.ConnectionId);
       //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
       GetClientHttpInfo();
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogWarning("{ContextConnectionId} покинул чат", Context.ConnectionId);
        //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
        await base.OnDisconnectedAsync(exception);
    }


    private void GetClientHttpInfo()
    {
        var context = Context.GetHttpContext();
        if(context is not null)
        {
            // получаем кук name
            if (context.Request.Cookies.ContainsKey("name"))
            {
                if (context.Request.Cookies.TryGetValue("name", out var userName))
                {
                    _logger.LogInformation("name = {UserName}", userName);
                }
            }
            // получаем юзер-агент
            _logger.LogInformation("UserAgent = {RequestHeader}", context.Request.Headers["User-Agent"]);
            // получаем ip
            _logger.LogInformation("RemoteIpAddress = {RemoteIpAddress}", context.Connection?.RemoteIpAddress?.ToString());
        }
    }
}