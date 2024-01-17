using Confluent.Kafka;

var type = typeof(Program);
var assemblyName= type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);

// var config = new ProducerConfig
// {
//     BootstrapServers = "localhost:29092",
// };
//
// // If serializers are not specified, default serializers from
// // `Confluent.Kafka.Serializers` will be automatically used where
// // available. Note: by default strings are encoded as UTF8.
// using var build = new ProducerBuilder<Null, string>(config).Build();
// Console.WriteLine("Producer start>>>>>>>>>>>");
// try
// {
//     for (int i = 0; i < 100; i++)
//     {
//         await Task.Delay(TimeSpan.FromSeconds(1));
//         var dr = await build.ProduceAsync("test-topic", new Message<Null, string> { Value=$"test {DateTime.Now:T}" });
//         Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
//     }
// }
// catch (ProduceException<Null, string> e)
// {
//     Console.WriteLine($"Delivery failed: {e.Error.Reason}");
// }


var t1=Task.Run(async () =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:29092",
        AllowAutoCreateTopics = true,
        
    };

// If serializers are not specified, default serializers from
// `Confluent.Kafka.Serializers` will be automatically used where
// available. Note: by default strings are encoded as UTF8.
    using var build = new ProducerBuilder<Null, string>(config).Build();
    Console.WriteLine("Producer start>>>>>>>>>>>");
    try
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var topicPart = new TopicPartition("test-topic", new Partition(0)); 
            var dr = await build.ProduceAsync(topicPart, new Message<Null, string> {Value=$"Task 1 {DateTime.Now:T}" });
            Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
    catch (ProduceException<string, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }
});

var t2=Task.Run(async () =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:29092",
        AllowAutoCreateTopics = true
    };

// If serializers are not specified, default serializers from
// `Confluent.Kafka.Serializers` will be automatically used where
// available. Note: by default strings are encoded as UTF8.
    using var build = new ProducerBuilder<Null, string>(config).Build();
    Console.WriteLine("Producer start>>>>>>>>>>>");
    try
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var topicPart = new TopicPartition("test-topic", new Partition(1)); 
            var dr = await build.ProduceAsync(topicPart, new Message<Null, string> {Value=$"Task 2 {DateTime.Now:T}" });
            Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
    catch (ProduceException<string, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }
});

await Task.WhenAll(t1, t2);


Console.ReadKey();