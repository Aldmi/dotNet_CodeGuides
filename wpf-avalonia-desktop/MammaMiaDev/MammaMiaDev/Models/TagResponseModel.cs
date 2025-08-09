using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MammaMiaDev.ViewModels;

namespace MammaMiaDev.Models;

public partial class TagResponseModel : ObservableObject
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
	private void GetNetworkSettings()
	{
		var dialog = new DialogNetworkSettingsViewModel
		{
			TagResponseModel = this
		};
        
		// Здесь нужно вызвать диалоговое окно
		// Например, через сервис диалогов или Messaging
	}
}