using Confluent.Kafka;

var type = typeof(Program);
var assemblyName= type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);

var period = TimeSpan.FromSeconds(1);


var producerTask1=Task.Run(async () =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:29092",
        AllowAutoCreateTopics = true,
    };
    
    using var build = new ProducerBuilder<Null, string>(config).Build();
    Console.WriteLine("1 Producer start>>>>>>>>>>>");
    try
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(period);
            var topicPart = new TopicPartition("test-topic", new Partition(0)); 
            //var dr = await build.ProduceAsync(topicPart, new Message<Null, string> {Value=$"Task 1 {DateTime.Now:T}" });
            var dr = await build.ProduceAsync("test-topic", new Message<Null, string> {Value=$"Task 1 {DateTime.Now:T}" });
            Console.WriteLine($"1 Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"1 Delivery failed: {e.Error.Reason}");
    }
});

var producerTask2= Task.Run(async () =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:29092",
        AllowAutoCreateTopics = true
    };
    
    using var build = new ProducerBuilder<Null, string>(config).Build();
    Console.WriteLine("2 Producer start>>>>>>>>>>>");
    try
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(period);
            var topicPart = new TopicPartition("test-topic", new Partition(0)); 
            //var dr = await build.ProduceAsync(topicPart, new Message<Null, string> {Value=$"Task 2 {DateTime.Now:T}" });
            var dr = await build.ProduceAsync("test-topic", new Message<Null, string> {Value=$"Task 2 {DateTime.Now:T}" });
            Console.WriteLine($"2 Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"2 Delivery failed: {e.Error.Reason}");
    }
});


var producerTask3= Task.Run(async () =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:29092",
        AllowAutoCreateTopics = true
    };
    
    using var build = new ProducerBuilder<Null, string>(config).Build();
    Console.WriteLine("3 Producer start>>>>>>>>>>>");
    try
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(period);
            var topicPart = new TopicPartition("test-topic", new Partition(0)); 
            //var dr = await build.ProduceAsync(topicPart, new Message<Null, string> {Value=$"Task 3 {DateTime.Now:T}" });
            var dr = await build.ProduceAsync("test-topic", new Message<Null, string> {Value=$"Task 3 {DateTime.Now:T}" });
            Console.WriteLine($"3 Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"2 Delivery failed: {e.Error.Reason}");
    }
});

await Task.WhenAll(producerTask1, producerTask2, producerTask3);
