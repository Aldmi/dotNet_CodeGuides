using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MammaMiaDev.Domain.Core.Services;
using MammaMiaDev.Models;
using MammaMiaDev.Services;
using MammaMiaDev.ViewModels;
using MammaMiaDev.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MammaMiaDev;

public partial class App : Application
{
	public static IServiceProvider Services { get; private set; } = null!;

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopLifetime)
		{
			base.OnFrameworkInitializationCompleted();
			return;
		}

		// 1. Инициализация сервисов
		var services = ConfigureServices();
    
		// 2. Создание главного окна
		var mainWindow = new MainWindow();
    
		// 3. Регистрация сервисов, зависящих от UI
		RegisterUiServices(services, mainWindow);
    
		// 4. Построение провайдера
		Services = services.BuildServiceProvider();
    
		// 5. Настройка главного окна
		desktopLifetime.MainWindow = mainWindow;
		mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();
    
		DisableAvaloniaDataAnnotationValidation();
		base.OnFrameworkInitializationCompleted();
	}

	private static ServiceCollection ConfigureServices()
	{
		var services = new ServiceCollection();

		// Регистрируем все ViewModel
		services.AddTransient<MainWindowViewModel>();
		services.AddTransient<HomePageViewModel>();
		services.AddTransient<ButtonPageViewModel>();
		services.AddTransient<ImagePageViewModel>();
		services.AddTransient<GridPageViewModel>();
		services.AddTransient<ListItemsPageViewModel>();
        
		// Регистрируем Model
		services.AddTransient<TagResponseModel>();
        
		// Регистрируем Service
		services.AddSingleton<IChangeNetworkSettingService, ChangeNetworkSettingService>();

		return services;
	}
	
	
	private static void RegisterUiServices(IServiceCollection services, Window mainWindow)
	{
		services.AddSingleton<IDialogService>(_ => new AvaloniaDialogService(mainWindow));
		// Другие UI-зависимые сервисы можно добавить здесь
	}
	
	private void DisableAvaloniaDataAnnotationValidation()
	{
		var dataValidationPluginsToRemove =
			BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
		
		foreach (var plugin in dataValidationPluginsToRemove)
		{
			BindingPlugins.DataValidators.Remove(plugin);
		}
	}
}