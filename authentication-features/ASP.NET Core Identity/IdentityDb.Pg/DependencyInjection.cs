using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityDb.Pg;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connection = configuration.GetConnectionString("IdentityDbConnection");
        services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connection));
        return services;
    }
}