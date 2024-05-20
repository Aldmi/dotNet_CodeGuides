using Confluent.Kafka;

var type = typeof(Program);
var assemblyName= type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);

var config = new ProducerConfig
{
    BootstrapServers = "localhost:29092",
    AllowAutoCreateTopics = true,
    Acks = Acks.Leader,
};

using var build = new ProducerBuilder<Null, string>(config).Build();
Console.WriteLine("Producer start>>>>>>>>>>>");
try
{
    for (int i = 0; i < 10; i++)
    {
        var dr = await build.ProduceAsync("test-topic", new Message<Null, string> { Value=$"test {DateTime.Now:T}" });
        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
catch (ProduceException<Null, string> e)
{
    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
}

Console.ReadKey();