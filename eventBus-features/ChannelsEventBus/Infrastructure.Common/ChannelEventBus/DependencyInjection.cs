using Application.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common.ChannelEventBus;

public static class DependencyInjection
{
    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryMessageQueue>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();
        return services;
    }
}