using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MammaMiaDev.Domain.Core.Services;
using MammaMiaDev.Models;
using MammaMiaDev.Services;

namespace MammaMiaDev.ViewModels;

public partial class DialogNetworkSettingsViewModel : ViewModelBase
{
	private readonly TagResponseModel _parentModel;
	private readonly IChangeNetworkSettingService _changeNetworkSettingService;

	[ObservableProperty] private Guid _tagId;

	[ObservableProperty]
	private Dictionary<string, string> _macAddressDict;

	public DialogNetworkSettingsViewModel(TagResponseModel parentModel, IChangeNetworkSettingService changeNetworkSettingService)
	{
		_parentModel = parentModel;
		_changeNetworkSettingService = changeNetworkSettingService;
		// Предзаполняем данные
		MacAddressDict = parentModel.MacAddressDict;
		TagId = parentModel.TagId;
	}

	[RelayCommand]
	private async Task ApplySettings()
	{
		//для теста вызовем команду
		_changeNetworkSettingService.SendChangeCommand();
		
		// Сохраняем изменения в родительской модели
		_parentModel.MacAddressDict = MacAddressDict;
		_parentModel.TagId = TagId;
        
		// Отправляем сообщение с результатом true (подтверждение)
		WeakReferenceMessenger.Default.Send(new CloseDialogMessage(true));
	}

	[RelayCommand]
	private void Cancel()
	{
		// Закрываем без сохранения
		WeakReferenceMessenger.Default.Send(new CloseDialogMessage(false));
	}
}


