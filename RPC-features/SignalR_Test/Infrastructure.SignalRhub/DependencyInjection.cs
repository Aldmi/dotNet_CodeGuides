using Infrastructure.SignalRhub.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SignalRhub;

public static class DependencyInjection
{
    public static IServiceCollection AddSignalRService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR().AddHubOptions<ChatHub>(options =>                                      //настроим конкретный хаб
        {
            //options.EnableDetailedErrors = true;
           //options.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
           options.ClientTimeoutInterval = TimeSpan.FromSeconds(10); //30 default
        });
        return services;
    }
    
    
    public static void MapSignalRHub(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        endpoints.MapHub<ChatHub>("/chat");   // ChatHub будет обрабатывать запросы по пути /chat
    }
}