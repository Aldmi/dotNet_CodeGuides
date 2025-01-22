using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Contracts;


public record TagPayload(string Name, string IpAddress, string MacAddress, DateTime CreatedAt)
{
	
	public static TagPayload Create(string name, string ipAddress, string macAddress)
	{
		var payload = new TagPayload(name, ipAddress, macAddress, DateTime.UtcNow);
		return payload;
	}
	
	public byte[] ToBuffer()
	{
		string formatString = $"{Name}_{IpAddress}_{MacAddress}_{CreatedAt}";
		return Encoding.ASCII.GetBytes(formatString);
	}
	
	
	public static TagPayload FromBuffer(byte[] buffer)
	{
		var str= Encoding.ASCII.GetString(buffer, 0, buffer.Length);
		var parts = str.Split('_');
		if (parts.Length != 4)
		{
			throw new ArgumentException("invalid buffer");
		}
		
		return new TagPayload(parts[0], parts[1], parts[2], DateTime.Parse(parts[3]));
	}
}