using Grpc.Core;

namespace GreeterServiceApp.Services;

using ServerStreamServiceApp;

public class ServerStreamService : Messenger.MessengerBase
{
    string[] messages = ["Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока", "Ну пока!!!"];


    public override async Task GetDataStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {
        try
        {
            foreach (var message in messages)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    return;
                }
                await responseStream.WriteAsync(new Response() {Content = message}, context.CancellationToken);
                Console.WriteLine($"ServerStreamService Write {message}");
                //await Task.Delay(TimeSpan.FromSeconds(1));
                await Task.Delay(TimeSpan.FromSeconds(1),context.CancellationToken);
            }
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine($"ServerStreamService Canceled {e.Message}");
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
        }
    }
}