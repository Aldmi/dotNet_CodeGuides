using System.Net;
using System.Text;
namespace Contracts;


public record ScannerPayload(string IpAddress, int ListenPortNumber, DateTime CreatedAt)
{
	public static ScannerPayload Create(string subNetworkAddress, int portNumber)
	{
		var localIp =  NetworkHelpers.GetLocalIpAddress(subNetworkAddress);
		var payload = new ScannerPayload(localIp, portNumber, DateTime.UtcNow);
		return payload;
	}
	
	public byte[] ToBuffer()
	{
		string formatString = $"{IpAddress}_{ListenPortNumber}_{CreatedAt}";
		return Encoding.ASCII.GetBytes(formatString);
	}
	
	public static ScannerPayload FromBuffer(byte[] buffer)
	{
		var str= Encoding.ASCII.GetString(buffer, 0, buffer.Length); //TODO: можно ли преобразовать в Span<string>
		var parts = str.Split('_');
		if (parts.Length != 3)
		{
			throw new ArgumentException("invalid buffer");
		}
		return new ScannerPayload(parts[0], int.Parse(parts[1]), DateTime.Parse(parts[2]));
	}
}