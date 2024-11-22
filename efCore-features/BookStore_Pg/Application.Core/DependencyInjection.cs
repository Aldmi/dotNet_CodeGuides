using System.Reflection;
using Application.Core.BookFeatures.Command.CreateBook;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        services.AddScoped<CreateBookService>();
        
        return services;
    }
}