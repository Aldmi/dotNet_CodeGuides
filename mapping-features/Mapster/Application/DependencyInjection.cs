using Application.DataAccess.Abstract;
using Application.Features.GetProduct;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetProductReadService>();
        
        return services;
    }
}