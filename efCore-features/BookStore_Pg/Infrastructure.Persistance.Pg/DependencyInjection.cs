using Application.Core.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Pg;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, PgDbOption option, Action<string>? logTo = null)
    {
        services.AddScoped<IBookContext, BookContext>(serviceProvider =>  new BookContext(option.ConnectionString, null, logTo)); // Можно создать в ручную сервисы зарегистрированные в DI, например serviceProvider.GetService<Mediator.Publisher>()
        return services;
    }
}