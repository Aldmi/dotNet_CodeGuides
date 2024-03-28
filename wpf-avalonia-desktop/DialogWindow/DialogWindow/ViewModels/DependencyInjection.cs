using DialogWindow.Models;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace DialogWindow.ViewModels;

public static class DependencyInjection {
    public static void AddVm(this IServiceCollection collection) {
         collection.AddTransient<MainWindowViewModel>();
         collection.AddTransient<DialogViewModel>();
         collection.AddTransient<Interaction<DialogViewModel, Persone?>>();
    }
}