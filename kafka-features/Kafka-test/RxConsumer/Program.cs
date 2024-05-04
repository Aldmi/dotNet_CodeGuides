using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using RxConsumer;

Console.WriteLine("Rx kafka Consumer");


const string topicName = "Test_1";
var consumer = new RxKafkaConsumer("localhost:29092", "test-consumer-group", topicName);

var result=consumer.ConsumeLastMessages(6);

//await new TaskCompletionSource<object>().Task;



//выполним отписку через 3 сек
//var cts = new CancellationTokenSource(); //TimeSpan.FromSeconds(3)
//var observable = consumer.Consume(cts.Token);
var observable = consumer.Consume_Ver2();
var subscription = observable?
    .Select(result =>
    {
        try
        {
            if (result.Value == "3") {
                throw new ApplicationException("sadsadsad");
            }
        }
        catch (Exception e)
        {
            return Result.Failure<string>(e.ToString());
        }

        return "NEW_" + result;
    })
    .Subscribe(
        message => { Console.WriteLine($"Consume: {message}"); },
        (e) => { Console.WriteLine($"Terminated ERROR on Program: {e}"); },
        () => { Console.WriteLine("completed"); }
    );


// await Task.Delay(TimeSpan.FromSeconds(5));
// subscription?.Dispose();
// // cts.Dispose();
// //
// await Task.Delay(TimeSpan.FromSeconds(5));
// Console.WriteLine("------NEW Subscribe");
// //
// // //Подпишемся заново через 5 сек
// observable = consumer.Consume_Ver2();
// subscription = observable?.Subscribe(
//     s =>
//     {
//         Console.WriteLine($"Consume: {s}");
//     },
//     (e) =>
//     {
//         Console.WriteLine($"ERROR on Program: {e}");
//     },
//     () =>
//     {
//         Console.WriteLine($"completed");
//     }
// );
Console.ReadKey();

//Отключить консюмера
//cts.Cancel();
await Task.Delay(500);
consumer.Dispose();

//await new TaskCompletionSource<object>().Task;