namespace MammaMiaDev.Domain.Core.Services;

public interface IChangeNetworkSettingService
{
	void SendChangeCommand();
}


public class ChangeNetworkSettingService : IChangeNetworkSettingService
{
	public void SendChangeCommand()
	{
	}
}