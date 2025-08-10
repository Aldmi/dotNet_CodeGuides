using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace MammaMiaDev.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	private readonly IServiceProvider _serviceProvider;
	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		CurrentPage = _serviceProvider.GetRequiredService<ListItemsPageViewModel>();
	}
	
	[ObservableProperty]
	private bool _isPanelOpen = true;

	[ObservableProperty]
	private ViewModelBase _currentPage;
	

	[ObservableProperty]
	private ListItemTemplate? _selectedListItem;

	partial void OnSelectedListItemChanged(ListItemTemplate? value)
	{
		if (value is null)
			return;
        
		CurrentPage = (ViewModelBase)_serviceProvider.GetRequiredService(value.ModelType);
	}
	
	public ObservableCollection<ListItemTemplate> Items { get; } = 
	[
		new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
		new ListItemTemplate(typeof(ButtonPageViewModel), "CursorHoverRegular"),
		new ListItemTemplate(typeof(ImagePageViewModel), "ImageLibraryRegular"),
		new ListItemTemplate(typeof(GridPageViewModel), "GridRegular"),
		new ListItemTemplate(typeof(ListItemsPageViewModel), "WindowInprivateRegular"),
	];
	
	[RelayCommand]
	private void TriggerPane()
	{
		IsPanelOpen= !IsPanelOpen;
	}
}

public class ListItemTemplate
{
	public ListItemTemplate(Type type, string iconKey)
	{
		ModelType = type;
		Label = type.Name.Replace("PageViewModel", string.Empty);
		
		Application.Current!.TryFindResource(iconKey, out var res);
		Icon = (StreamGeometry)res!;
	}
	
	public Type ModelType { get;  }
	public string Label { get; }
	public StreamGeometry Icon { get; set; }
}