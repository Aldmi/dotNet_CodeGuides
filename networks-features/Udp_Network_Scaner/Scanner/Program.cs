using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;

/*
 Запускает 2 параллельные задачи
 1. Прослушивает Udp socket на указанном порту
       --- Ждет ответа от устройств

 2. Шлет в цикле запрос scanQuery
        --- Запрос определенного формата (в котором указан Ip и port для ответа) БРОАДКАСТ в ОТВЕТЕ НЕ нужен
 */


Console.WriteLine("Scanner Starting....");
var cts = new CancellationTokenSource(); //TODO: сработка токена по нажатию кнопки 'q' в консоли

//From settings----------------------------------------------------------
string subNetworkAddress = "192.168.1";
const int listenPort = 11001;
int portNumber = 11000;
var payload = ScannerPayload.Create(subNetworkAddress, listenPort);
TimeSpan scanPeriod = TimeSpan.FromSeconds(1);


//Sender-----------------------------------------------------------------
Task scanQueryTask = Task.Factory.StartNew(async () =>
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
			{ EnableBroadcast = true };

		IPEndPoint broadcast = new IPEndPoint(IPAddress.Parse(subNetworkAddress + ".255"), portNumber);
		while (!cts.IsCancellationRequested)
		{
			var sendBytes = await socket.SendToAsync(payload.ToBuffer(), broadcast, cts.Token); //TODO: cts.Token - как правильно завершать
			await Task.Delay(scanPeriod, cts.Token);
			Console.WriteLine($"Sending message to listener {sendBytes}");
		}

		socket.Dispose();
	},
	cts.Token,
	TaskCreationOptions.LongRunning,
	TaskScheduler.Default);


//Listener---------------------------------------------------------------
Task scanResponseTask = Task.Factory.StartNew(async () =>
	{
		UdpClient listener = new UdpClient(listenPort) { EnableBroadcast = true };
		//IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, listenPort);
		IPEndPoint groupEp = new IPEndPoint(IPAddress.Parse(payload.IpAddress), listenPort);  //слушаю на своем Ip конкретный порт для ответа

		try
		{
			while (!cts.IsCancellationRequested)
			{
				Console.WriteLine("Waiting tag response");
				byte[] bytes = listener.Receive(ref groupEp);    //TODO: заменить на ReceiveAsync

				//Обработка ответа
				var payload = TagPayload.FromBuffer(bytes);
				Console.WriteLine($"---------------------------------------");
				Console.WriteLine($"Received response from TAG '{payload}'");
				Console.WriteLine($"---------------------------------------\n\n");
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
	await Task.WhenAll(scanQueryTask, scanResponseTask);
}
catch (Exception e)
{
	Console.WriteLine(e);
}

Console.WriteLine("Scanner Stopped");
Console.ReadKey();
