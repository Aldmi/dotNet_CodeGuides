using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Tag Starting....");

var cts = new CancellationTokenSource(); //TODO: сработка токена по нажатию кнопки 'q' в консоли

const int listenPort = 11000;
Task tagTask = Task.Factory.StartNew(async () =>
	{
		UdpClient listener = new UdpClient(listenPort) { EnableBroadcast = true };
		IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, listenPort); //Принимаю broadcast
		try
		{
			while (true)
			{
				Console.WriteLine("Waiting scanner query");
				byte[] bytes = listener.Receive(ref groupEp); //TODO: заменить на ReceiveAsync

				//Обработка broadcast сообщения
				Console.WriteLine($"Received broadcast from scanner {groupEp} :");
				Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
				
				string scannerPayload = "MacAddress, .....";
				IPEndPoint epScanner = new IPEndPoint(IPAddress.Parse("192.168.1.255"), 11001); //берет из запроса ip сканера и порт (куда отправить ответ)
				await SendPayload(epScanner, scannerPayload);
			}
		}
		catch (SocketException e)
		{
			Console.WriteLine(e);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			listener.Close();
		}
	},
	cts.Token,
	TaskCreationOptions.LongRunning,
	TaskScheduler.Default);


try
{
	await tagTask;
}
catch (Exception e)
{
	Console.WriteLine(e);
}

Console.WriteLine("Tag Stopped");
Console.ReadKey();



async ValueTask SendPayload(IPEndPoint ep, string payload)
{
	using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
		{ EnableBroadcast = true };
	byte[] buffer = Encoding.ASCII.GetBytes(payload);
	
	var sendBytes = await socket.SendToAsync(buffer, ep, cts.Token); //TODO: cts.Token - как правильно завершать
	Console.WriteLine($"Sending message to scanner {sendBytes}");
}