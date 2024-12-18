using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var sumAss= new List<Assembly>(assemblies){assembly};
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(sumAss.ToArray())); 
        return services;
    }
}