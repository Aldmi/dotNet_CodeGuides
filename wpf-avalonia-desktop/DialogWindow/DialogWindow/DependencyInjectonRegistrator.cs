using DialogWindow.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DialogWindow;

public static class DependencyInjectonRegistrator
{
    /// <summary>
    /// Тут регистрировать все сервисы необходимые для работы приложения
    /// </summary>
    public static ServiceCollection RegisterAllServices()
    {
        var collection = new ServiceCollection();
        //collection.AddPersistance();
        collection.AddVm();
        return collection;
    }
}