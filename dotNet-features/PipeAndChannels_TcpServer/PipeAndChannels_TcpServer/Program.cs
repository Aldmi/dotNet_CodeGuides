using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
	e.Cancel = true;
	cts.Cancel();
};


var listener = new TcpListener(IPAddress.Any, 5000);
listener.Start();
Console.WriteLine($"Listening on port 5000");


var channel = Channel.CreateUnbounded<string>();

//запускаем обработчик сообщений из Channel
_ = Task.Run(async () =>
{
	await foreach (var message in channel.Reader.ReadAllAsync())
	{
		Console.WriteLine($"Обработано сообщение: {message}");
	}
});


try
{
	// Обработка tcp client
	while (!cts.Token.IsCancellationRequested)
	{
		var client = await listener.AcceptTcpClientAsync(cts.Token);
		Console.WriteLine($"Tcp клиент подключен {client.Client.RemoteEndPoint}");
		_ = HandleClient(client, channel.Writer, cts.Token);
	}
}
catch (OperationCanceledException)
{
	Console.WriteLine("Сервер остановлен");
}
finally
{
	listener.Stop();
	channel.Writer.Complete();
}

return 0;


async Task HandleClient(TcpClient client, ChannelWriter<string> channelWriter, CancellationToken ct)
{
	try
	{
		using (client)
		await using (var stream = client.GetStream())
		{
			var pipe = new Pipe();
			_ = FillPipe(stream, pipe.Writer, ct);
            
			while (true)
			{
				var result = await pipe.Reader.ReadAsync(ct);
				var buffer = result.Buffer;

				//Обрабатываем полученный буфер (он состоит из кусочков)
				while (true)
				{
					var position = buffer.PositionOf((byte)'1'); // (в конце строки находится /r/n) если нажали enter, то отправляем полученную строку в channelWriter
					if (position == null)
						break;
			
					var line = buffer.Slice(0, position.Value);
					var message= Encoding.UTF8.GetString(line.ToArray());
					await channelWriter.WriteAsync(message, ct);

					var next = buffer.GetPosition(1, position.Value);//это эквивалентно position + 1
					buffer = buffer.Slice(next); //пропустить то, что мы уже обработали, включая '\n'
				}
		
				pipe.Reader.AdvanceTo(buffer.Start, buffer.End);
				Console.WriteLine($"Reader.AdvanceTo {buffer.Start.GetInteger()} - {buffer.End.GetInteger()}");
		
				if(result.IsCompleted)
					break;
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Ошибка обработки клиента: {ex.Message}");
	}
}


async Task FillPipe(NetworkStream stream, PipeWriter writer, CancellationToken ct)
{
	try
	{
		while (!ct.IsCancellationRequested)
		{
			var memory = writer.GetMemory(512); // Буфер расширяется при необходимости
			var bytesRead = await stream.ReadAsync(memory, ct);
			if (bytesRead == 0)
				break;

			writer.Advance(bytesRead);
			await writer.FlushAsync(ct);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Ошибка чтения из потока: {ex.Message}");
	}
	finally
	{
		await writer.CompleteAsync();
	}
}

