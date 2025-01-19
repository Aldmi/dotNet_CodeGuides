using System.Net;
using System.Net.Sockets;
using System.Text;

Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp){EnableBroadcast = true};  
  
IPAddress broadcast = IPAddress.Parse("192.168.1.255");  
  
byte[] sendbuf = Encoding.ASCII.GetBytes("Hello World!");  
IPEndPoint ep = new IPEndPoint(broadcast, 11000);


while (true)
{
	s.SendTo(sendbuf, ep);
	await Task.Delay(1000);
	Console.WriteLine("Sending message to listener");
}

