using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MammaMiaDev.Domain.Core.Services;
using MammaMiaDev.Services;
using MammaMiaDev.ViewModels;

namespace MammaMiaDev.Models;

public partial class TagResponseModel(IDialogService dialogService, IChangeNetworkSettingService changeNetworkSettingService) : ObservableObject
{
	[ObservableProperty] private Guid _tagId;

	private Dictionary<string, string> _macAddressDict = new();
	public Dictionary<string, string> MacAddressDict
	{
		get => _macAddressDict;
		set
		{
			if (SetProperty(ref _macAddressDict, value))
			{
				OnPropertyChanged(nameof(MacAddressesDisplay));
			}
		}
	}

	[ObservableProperty] private DateTime _createdAtUtc;

	[ObservableProperty] private List<TagIpModel> _tagIpModels;

	public string MacAddressesDisplay => string.Join(", ", MacAddressDict.Select(x => $"{x.Key}: {x.Value}"));
	
	
	[RelayCommand]
	private async Task GetNetworkSettings()
	{
		var dialogVm = new DialogNetworkSettingsViewModel(this, changeNetworkSettingService);
		var result = await dialogService.ShowDialogAsync(dialogVm);
        
		if (result == true)
		{
			// Диалог был подтвержден
			// Данные уже сохранены через _parentModel в ApplySettings()
		}
		else
		{
			// Диалог был отменен
		}
		
		// Изменения уже сохранены через связь с родительской моделью
		// Можно добавить дополнительную логику после закрытия
	}
}