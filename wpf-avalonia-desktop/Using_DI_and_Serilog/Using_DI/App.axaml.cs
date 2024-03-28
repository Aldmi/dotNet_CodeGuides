using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Using_DI.ViewModels;
using Using_DI.Views;

namespace Using_DI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        
        // Register all the services needed for the application to run
        var collection = DependencyInjectonRegistrator.RegisterAllServices();
        
        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();
        
        var vm = services.GetRequiredService<MainWindowViewModel>(); //Создать root объект через DI, что создаст весь граф необходимых объектов. services (IoC контейнер при этом удерживается в памяти, т.к. выданный граф объектов удерживает его в памяти.)
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}