using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;

Console.WriteLine("Tag Starting....");
var cts = new CancellationTokenSource(); //TODO: сработка токена по нажатию кнопки 'q' в консоли

//From settings----------------------------------------------------------
const int listenPort = 11000;
const string tagName = "Device1";

//Init-----------------------------------------------------------------
var macAddress = "10-20-30-40-5F-FF";
var tagIpAddress= NetworkHelpers.GetLocalIpAddress("192.168.1");

//Listener---------------------------------------------------------------
Task tagTask = Task.Factory.StartNew(async () =>
	{
		UdpClient listener = new UdpClient(listenPort) { EnableBroadcast = true };
		IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, listenPort); //Принимаю broadcast
		try
		{
			while (!cts.IsCancellationRequested)
			{
				Console.WriteLine("Waiting scanner query");
				byte[] bytes = listener.Receive(ref groupEp); //TODO: заменить на ReceiveAsync
				
				Console.WriteLine($"Received broadcast from scanner {groupEp} :");
                //Обработка broadcast сообщения от scanner
				var scannerPayload= ScannerPayload.FromBuffer(bytes);
				Console.WriteLine($"ScannerPayload {scannerPayload}");

				//Отправка ответа сканеру.
				IPEndPoint epScanner = new IPEndPoint(IPAddress.Parse(scannerPayload.IpAddress), scannerPayload.ListenPortNumber); //берет из запроса ip сканера и порт (куда отправить ответ)
				var tagPayload = TagPayload.Create(tagName, tagIpAddress, macAddress);
				await SendPayload(epScanner, tagPayload);
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



async ValueTask SendPayload(IPEndPoint ep, TagPayload tagPayload)
{
	using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
		{ EnableBroadcast = true };
	byte[] buffer = tagPayload.ToBuffer();
	
	var sendBytes = await socket.SendToAsync(buffer, ep, cts.Token); //TODO: cts.Token - как правильно завершать
	Console.WriteLine($"Sending message to scanner {sendBytes}");
}