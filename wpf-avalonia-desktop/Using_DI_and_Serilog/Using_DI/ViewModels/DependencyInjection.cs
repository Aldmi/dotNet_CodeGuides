using Microsoft.Extensions.DependencyInjection;

namespace Using_DI.ViewModels;

public static class DependencyInjection {
    public static void AddVm(this IServiceCollection collection) {
         collection.AddTransient<MainWindowViewModel>();
    }
}