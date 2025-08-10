using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace MammaMiaDev.Services;

public class AvaloniaDialogService : IDialogService
 {
 	private readonly Window _parentWindow;
 
 	public AvaloniaDialogService(Window parentWindow)
 	{
 		_parentWindow = parentWindow ?? throw new ArgumentNullException(nameof(parentWindow));
 	}
 	
 	
 	public async Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class
 	{
 		// Получаем тип View по соглашению об именовании
 		var viewType = Type.GetType(viewModel.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal));
     
 		if (viewType == null)
 			throw new InvalidOperationException($"View not found for {viewModel.GetType().Name}");
 		
 		var content = (Control)ActivatorUtilities.CreateInstance(App.Services, viewType);
 		content.DataContext = viewModel;
 
 		// Создаем окно-обертку
 		var dialog = new DialogWindow
 		{
 			Content = content,
 		};
 
 		return await dialog.ShowDialog<bool?>(_parentWindow);
 	}
 }