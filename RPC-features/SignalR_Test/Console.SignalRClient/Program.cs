
/*
 * SignalR клиент
 */


using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using Console.SignalRClient;

var allEnvs= Environment.GetEnvironmentVariables();
var env= Environment.GetEnvironmentVariable("SIGNALR_CLIENT_ID");


var connection = // подключение для взаимодействия с хабом
    // создаем подключение к хабу
    new HubConnectionBuilder()
    .WithUrl("https://localhost:5062/chat")
    .WithAutomaticReconnect()   // автопереподключение
    .ConfigureLogging(logging => 
    {
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .Build();

// регистрируем функцию Receive для получения данных
IDisposable unsub=  connection.On<Persone>("Receive", persone =>
{
    var newMessage = $"{persone.Age}: {persone.RegisterData}";
    System.Console.WriteLine($"{newMessage}");
});

connection.Reconnecting += (exception) =>
{
    System.Console.WriteLine($"Connection started reconnecting due to an error: {exception}");
    return Task.CompletedTask;
};

connection.Reconnected += (connectionId) =>
{
    System.Console.WriteLine($"Connection successfully reconnected. The ConnectionId is now: {connectionId}");
    return Task.CompletedTask;
};

connection.Closed += exception =>
{
    if (exception == null)
    {
        System.Console.WriteLine("Connection closed without error.");
    }
    else
    {
        System.Console.WriteLine($"Connection closed due to an error: {exception}");
    }
    return Task.CompletedTask;
};


try
{
// подключемся к хабу
    await connection.StartAsync();
    System.Console.WriteLine("Connection Ok");
}
catch (Exception e)
{
    System.Console.WriteLine(e);
    throw;
}


//Client send
// for (int i = 0; i < 100; i++)
// {
//     if (connection.State == HubConnectionState.Connected)
//     {
//         await connection.SendAsync("Send", $"mess {i}");
//         //await connection.InvokeAsync("Send", $"mess {i}");
//     }
//     await Task.Delay(TimeSpan.FromSeconds(1));
// }

System.Console.ReadKey();
await connection.StopAsync();
await connection.DisposeAsync();
