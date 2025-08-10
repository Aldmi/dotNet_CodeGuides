using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MammaMiaDev.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MammaMiaDev;

public class ViewLocator : IDataTemplate
{
	public Control? Build(object? param)
	{
		if (param is null)
			return null;

		var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
		var type = Type.GetType(name);

		if (type != null)
		{
			try
			{
				// Используем DI для создания View
				return (Control)ActivatorUtilities.CreateInstance(App.Services!, type);
			}
			catch
			{
				// Fallback для дизайна
				return (Control)Activator.CreateInstance(type)!;
			}
		}

		return new TextBlock { Text = "Not Found: " + name };
	}

	public bool Match(object? data)
	{
		return data is ViewModelBase;
	}
}