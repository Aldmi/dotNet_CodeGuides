using App.Core.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddAppCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<TimedBackgroundService>();
        return services;
    }
}