using System.Net;
using System.Net.Sockets;
using System.Text;

const int listenPort = 11000;  

StartListener();
Console.WriteLine("Stop Listening");
return;


static void StartListener()  
{ 
	UdpClient listener = new UdpClient(listenPort) {EnableBroadcast = true};  
	IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);  
	//IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("192.168.1.35"), listenPort);  
  
	try  
	{  
		while (true)  
		{  
			Console.WriteLine("Waiting for broadcast");  
			byte[] bytes = listener.Receive(ref groupEP);  
  
			Console.WriteLine($"Received broadcast from {groupEP} :");  
			Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");  
		}  
	}  
	catch (SocketException e)  
	{  
		Console.WriteLine(e);  
	}  
	finally  
	{  
		listener.Close();  
	}  
} 