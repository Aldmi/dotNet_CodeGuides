// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC

using GreeterClientApp;
using Grpc.Core; //указывается этот namespace в greet.proto (кодогенератор создаст все классы клиента там)
using Grpc.Net.Client;
using InviterClientApp;
using ServerStreamServiceApp;
using Request = InviterClientApp.Request;

using var channel = GrpcChannel.ForAddress("http://localhost:5098");


//Если сервер не доступен, то ошибка будет при вызове самого RPC метода (никакого реконнекта выполнять не надо)
// создаем клиент
//var client = new Greeter.GreeterClient(channel);
// try
// {
// Console.Write("Введите имя: ");
// string? name = Console.ReadLine();
//     // обмениваемся сообщениями с сервером
//     var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
//     Console.WriteLine($"Ответ сервера: {reply.Message}");
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
// }

//----------------------

// char cancelled = ' ';
// while (cancelled != 'x')
// {
//     Console.Write("Введите число 1: ");
//     int.TryParse(Console.ReadLine(), out var op1);
//     Console.Write("Введите число 2: ");
//     int.TryParse(Console.ReadLine(), out var op2);
//     Console.Write("Для отмены введите 'x' ");
//     char.TryParse(Console.ReadLine(), out cancelled);
// // обмениваемся сообщениями с сервером
//     try
//     { 
//         var reply_sum = await client.CalcSumAsync(new  SumRequest{ Val1 = op1, Val2 = op2 });
//         Console.WriteLine($"Ответ сервера: {reply_sum.Sum}");
//     }
//     catch (Exception e)
//     {
//         Console.WriteLine(e);
//     }
// }


//////////////////////////////
// var client_inviter = new Inviter.InviterClient(channel);
//
// var replyInvieter = await client_inviter.InviteAsync(new Request {Name = "Alex"});
// var eventInvitation = replyInvieter.Invitation;
// var eventDateTime = replyInvieter.Start.ToDateTime();
// var eventDuration = replyInvieter.Duration.ToTimeSpan();
//
// Console.WriteLine(eventInvitation);
// Console.WriteLine($"Начало: {eventDateTime:dd.MM HH:mm}   Длительность: {eventDuration.TotalHours} часа");


//////////////////////////////
var serverStreamClient= new Messenger.MessengerClient(channel);


while (true)
{
    //cancellationToken обрывает выполнение метода
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
    try
    {
        var serverData = serverStreamClient.GetDataStream(new ServerStreamServiceApp.Request(), cancellationToken: cts.Token);
        // получаем поток сервера
        var responseStream = serverData.ResponseStream;
        //var stream =  responseStream.ReadAllAsync().Take(2);//На стороне приемника мы можем ограничить кол-во данных, но сервер продолжит выдавать данные в поток
        var stream = responseStream.ReadAllAsync(); //cancellationToken: cts.Token
        await foreach (var response in stream)
        {
            var content = response.Content;
            Console.WriteLine($"Content from stream {content}");
        }
    }
    catch (OperationCanceledException e)
    {
        Console.WriteLine(e);
    }
    catch (RpcException e)
    {
        Console.WriteLine(e);
    }
    finally
    {
        cts.Dispose();
    }
}





Console.Write("Для завершения программы нажмите любую клавишу");
Console.ReadKey();