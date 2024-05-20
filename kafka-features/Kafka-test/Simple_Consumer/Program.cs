using Confluent.Kafka;

var type = typeof(Program);
var assemblyName= type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);


//AutoOffsetReset.Earliest - Чтение осуществляется с последнего НЕ ЗАКОМИЧЕННОГО message в топике
//AutoOffsetReset.Latest - Чтение осуществляется с offset + 1. Т.е. Ждем только новые сообщения с топика. НЕ ЗАКОМИЧЕННОГО message в топике (не считанные консьюмерами их этой консьюмер группы) игнорируются. 
var conf = new ConsumerConfig
{ 
    GroupId = "test-consumer-group_1",
    BootstrapServers = "localhost:29092",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true
};


using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
consumer.Subscribe("test-topic");

Console.WriteLine("Consumer start>>>>>>>>>>>");

while (Console.ReadKey().Key != ConsoleKey.E) //Чтение осуществляем по нажатию любой клваиши. E - выход
{
    try 
    {
        //Блокировка.
        var cr = consumer.Consume(CancellationToken.None);
        if (cr is not null)
        {
            Console.WriteLine($"Consumed message '{cr.Message.Value}'  '{cr.Message.Key}' at: '{cr.TopicPartitionOffset}'.");
        }
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Consumer Error occured: {e.Error.Reason}");
    }
}

//После работы коньсюмера ОБЯЗАТЕЛЬНО вызывать Close, чтобы сообщить консбюмер группе, что консьюмер завершил свою работу.
//Это уменьшит время на ребалансировку группы при последующем подключении консьюмера.
consumer.Close();