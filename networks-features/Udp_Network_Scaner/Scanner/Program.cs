using System.Net;
using System.Net.Sockets;
using System.Text;

/*
 Запускает 2 параллельные задачи
 1. Прослушивает Udp socket на указанном порту
       --- Ждет ответа от устройств

 2. Шлет в цикле запрос scanQuery
        --- Запрос определенного формата (в котором указан Ip и port для ответа) БРОАДКАСТ в ОТВЕТЕ НЕ нужен
 */


Console.WriteLine("Scanner Starting....");

var cts = new CancellationTokenSource(); //TODO: сработка токена по нажатию кнопки 'q' в консоли

//Sender-----------------------------------------------------------------
var query = "Hello World!";
IPAddress ipBroadcast = IPAddress.Parse("192.168.1.255");
int port = 11000;
TimeSpan queryPeriod = TimeSpan.FromSeconds(1);

Task scanQueryTask = Task.Factory.StartNew(async () =>
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
			{ EnableBroadcast = true };
		byte[] buffer = Encoding.ASCII.GetBytes(query);
		IPEndPoint ep = new IPEndPoint(ipBroadcast, port);

		while (!cts.IsCancellationRequested)
		{
			var sendBytes = await socket.SendToAsync(buffer, ep, cts.Token); //TODO: cts.Token - как правильно завершать
			await Task.Delay(queryPeriod, cts.Token);
			Console.WriteLine($"Sending message to listener {sendBytes}");
		}

		socket.Dispose();
	},
	cts.Token,
	TaskCreationOptions.LongRunning,
	TaskScheduler.Default);


const int listenPort = 11001;
Task scanResponseTask = Task.Factory.StartNew(async () =>
	{
		UdpClient listener = new UdpClient(listenPort) { EnableBroadcast = true };
		//IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, listenPort);
		IPEndPoint groupEp = new IPEndPoint(IPAddress.Parse("192.168.1.35"), listenPort);  //слушаю на своем Ip конкретный порт для ответа

		try
		{
			while (true)
			{
				Console.WriteLine("Waiting tag response");
				byte[] bytes = listener.Receive(ref groupEp);    //TODO: заменить на ReceiveAsync

				//Обработка ответа
				var payload = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
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
