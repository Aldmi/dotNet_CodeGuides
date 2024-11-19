
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Pg;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, PgDbOption option, Action<string>? logTo = null)
    {
        services.AddScoped<BookContext>(_ => new BookContext(option.ConnectionString, logTo));
        return services;
    }
}