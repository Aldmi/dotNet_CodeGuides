using Microsoft.Extensions.DependencyInjection;
using Using_DI.Services;
using Using_DI.ViewModels;

namespace Using_DI;

public static class DependencyInjectonRegistrator
{
    /// <summary>
    /// Тут регистрировать все сервисы необходимые для работы приложения
    /// </summary>
    public static ServiceCollection RegisterAllServices()
    {
        var collection = new ServiceCollection();
        collection.AddPersistance();
        collection.AddVm();
        return collection;
    }
}