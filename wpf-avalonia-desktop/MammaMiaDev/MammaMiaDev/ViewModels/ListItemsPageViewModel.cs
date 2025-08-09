using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using MammaMiaDev.Models;

namespace MammaMiaDev.ViewModels;

public class ListItemsPageViewModel : ViewModelBase
{
	public ReadOnlyObservableCollection<TagResponseModel> FilteredScannerResultList =>
		new ReadOnlyObservableCollection<TagResponseModel>(
			[
				new TagResponseModel()
				{
					TagId = Guid.NewGuid(),
					CreatedAtUtc = DateTime.UtcNow,
					MacAddressDict = new Dictionary<string, string>()
					{ 
						{"Mac 1", "00:1B:44:67:89:AB"},
						{"Mac 2", "11:22:44:67:89:AB"}
					},
					TagIpModels = new List<TagIpModel>()
					{
						new TagIpModel(IPAddress.Parse("192.168.1.1")),
						new TagIpModel(IPAddress.Parse("192.168.1.100"))
					}
				},
				
				new TagResponseModel()
				{
					TagId = Guid.NewGuid(),
					CreatedAtUtc = DateTime.UtcNow,
					MacAddressDict = new Dictionary<string, string>()
					{ 
						{"Mac 10", "10:1B:44:67:89:AB"},
						{"Mac 22", "20:22:44:67:89:AB"}
					},
					TagIpModels = new List<TagIpModel>()
					{
						new TagIpModel(IPAddress.Parse("10.168.1.1")),
						new TagIpModel(IPAddress.Parse("10.168.1.100"))
					}
				}
			]);
}