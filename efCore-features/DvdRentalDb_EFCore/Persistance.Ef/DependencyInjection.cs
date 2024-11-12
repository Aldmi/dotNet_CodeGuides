using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance.Ef;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DvdRentalDbContext>((sp, options) =>
        {
            options.UseNpgsql(connection);
        });
        
        return services;
    }
}