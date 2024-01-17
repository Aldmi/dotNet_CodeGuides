using Confluent.Kafka;

var type = typeof(Program);
var assemblyName= type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);

var period = TimeSpan.FromSeconds(1);


var consumerTask1 = Task.Run(async () =>
{
    var conf = new ConsumerConfig
    { 
        GroupId = "test-consumer-group",
        BootstrapServers = "localhost:29092",
        AutoOffsetReset = AutoOffsetReset.Latest,
        AllowAutoCreateTopics = true
    };
    
    using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
    consumer.Subscribe("test-topic");
    
    Console.WriteLine("1 Consumer start>>>>>>>>>>>");
    try
    {
        while (true)
        {
            try
            {
                await Task.Delay(period, CancellationToken.None);
                //Блокировка.
                var cr = consumer.Consume(CancellationToken.None); 
                Console.WriteLine($"1 Consumed message '{cr.Value}'  '{cr.Key}' at: '{cr.TopicPartitionOffset}'.");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"1 Error occured: {e.Error.Reason}");
            }
        }
    }
    catch (OperationCanceledException)
    {
        consumer.Close();
    }
});

var consumerTask2 = Task.Run(async () =>
{
    var conf = new ConsumerConfig
    { 
        GroupId = "test-consumer-group",
        BootstrapServers = "localhost:29092",
        AutoOffsetReset = AutoOffsetReset.Latest,
        AllowAutoCreateTopics = true,
    };
    
    using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
    consumer.Subscribe("test-topic");
    
    Console.WriteLine("2 Consumer start>>>>>>>>>>>");
    try
    {
        while (true)
        {
            try
            {
                await Task.Delay(period, CancellationToken.None);
                //Блокировка.
                var cr = consumer.Consume(CancellationToken.None); 
                Console.WriteLine($"2 Consumed message '{cr.Value}'  '{cr.Key}' at: '{cr.TopicPartitionOffset}'.");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"2 Error occured: {e.Error.Reason}");
            }
        }
    }
    catch (OperationCanceledException)
    {
        consumer.Close();
    }
});


var consumerTask3 = Task.Run(async () =>
{
    var conf = new ConsumerConfig
    { 
        GroupId = "test-consumer-group",
        BootstrapServers = "localhost:29092",
        AutoOffsetReset = AutoOffsetReset.Latest,
        AllowAutoCreateTopics = true
    };
    
    using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
    consumer.Subscribe("test-topic");
    
    Console.WriteLine("3 Consumer start>>>>>>>>>>>");
    try
    {
        while (true)
        {
            try
            {
                await Task.Delay(period, CancellationToken.None);
                //Блокировка.
                var cr = consumer.Consume(CancellationToken.None); 
                Console.WriteLine($"3 Consumed message '{cr.Value}'  '{cr.Key}' at: '{cr.TopicPartitionOffset}'.");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"3 Error occured: {e.Error.Reason}");
            }
        }
    }
    catch (OperationCanceledException)
    {
        consumer.Close();
    }
});


await Task.WhenAll(consumerTask1, consumerTask2, consumerTask3);




