using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MammaMiaDev.Models;

public partial class TagIpModel : ObservableObject
{
	//public SshService SshService { get; }

	public TagIpModel(IPAddress tagRawIpAddress)  //, SshService sshService
	{
		_ipAddressesStr = tagRawIpAddress.ToString();
		//SshService = sshService;
	}

	[ObservableProperty] private string _ipAddressesStr;

	[RelayCommand]
	private async Task HandleIpButtonClick(Control sender)
	{
		// const string defaultUserName = "root";
		// var parentWindow = (Window)sender.GetVisualRoot()!;
		// var dialog = new SshDialog(IpAddressesStr, defaultUserName);
		// var username = await dialog.ShowDialog<string?>(parentWindow);
		// if (!string.IsNullOrWhiteSpace(username))
		// {
		// 	//_ipAddressesStr = "10.1.26.130";
		// 	var res = SshService.LaunchWindowsSsh(IpAddressesStr, username);
		// }

	}
}