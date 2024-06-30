using Application.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture.DataAccess.Pg;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IDbContext, AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connection);
        });
        
        return services;
    }
}