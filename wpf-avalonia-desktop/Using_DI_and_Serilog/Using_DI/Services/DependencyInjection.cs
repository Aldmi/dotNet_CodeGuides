using Microsoft.Extensions.DependencyInjection;

namespace Using_DI.Services;

public static class DependencyInjection {
    public static void AddPersistance(this IServiceCollection collection) {
        collection.AddSingleton<IRepository, Repository>();
    }
}