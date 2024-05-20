using Confluent.Kafka;
using Confluent.Kafka.Admin;

var type = typeof(Program);
var assemblyName = type.Assembly.GetName().Name;
Console.WriteLine(assemblyName);

static string ToString(int[] array) => $"[{string.Join(", ", array)}]";

static void ListConsumerGroups(string bootstrapServers)
{
    using (var adminClient =
           new AdminClientBuilder(new AdminClientConfig {BootstrapServers = bootstrapServers}).Build())
    {
        // Warning: The API for this functionality is subject to change.
        try
        {
            var groups =
                adminClient.ListGroups(TimeSpan
                    .FromSeconds(1)); //TimeSpan.FromSeconds(0.001) - выдается KafkaException "Local timeout"
            Console.WriteLine($"Consumer Groups:");
            foreach (var g in groups)
            {
                Console.WriteLine($"  Group: {g.Group} {g.Error} {g.State}");
                Console.WriteLine($"  Broker: {g.Broker.BrokerId} {g.Broker.Host}:{g.Broker.Port}");
                Console.WriteLine($"  Protocol: {g.ProtocolType} {g.Protocol}");
                Console.WriteLine($"  Members:");
                foreach (var m in g.Members)
                {
                    Console.WriteLine($"    {m.MemberId} {m.ClientId} {m.ClientHost}");
                    Console.WriteLine($"    Metadata: {m.MemberMetadata.Length} bytes");
                    Console.WriteLine($"    Assignment: {m.MemberAssignment.Length} bytes");
                }
            }
        }
        catch (KafkaException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

//Данные про топики и партиции в топиках на брокере
static void PrintMetadata(string bootstrapServers)
{
    using (var adminClient =
           new AdminClientBuilder(new AdminClientConfig {BootstrapServers = bootstrapServers}).Build())
    {
        // Warning: The API for this functionality is subject to change.
        var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(1));
        Console.WriteLine($"{meta.OriginatingBrokerId} {meta.OriginatingBrokerName}");
        meta.Brokers.ForEach(broker => Console.WriteLine($"Broker: {broker.BrokerId} {broker.Host}:{broker.Port}"));

        meta.Topics.ForEach(topic =>
        {
            Console.WriteLine($"Topic: {topic.Topic} {topic.Error}");
            topic.Partitions.ForEach(partition =>
            {
                Console.WriteLine($"  Partition: {partition.PartitionId}");
                Console.WriteLine($"    Replicas: {ToString(partition.Replicas)}");
                Console.WriteLine($"    InSyncReplicas: {ToString(partition.InSyncReplicas)}");
            });
        });
    }
}


static void PrintTopicDescriptions(List<TopicDescription> topicDescriptions, bool includeAuthorizedOperations)
{
    foreach (var topic in topicDescriptions)
    {
        Console.WriteLine($"\n  Topic: {topic.Name} {topic.Error}");
        Console.WriteLine($"  Topic Id: {topic.TopicId}");
        Console.WriteLine($"  Partitions:");
        foreach (var partition in topic.Partitions)
        {
            Console.WriteLine($"    Partition ID: {partition.Partition} with leader: {partition.Leader}");
            if (!partition.ISR.Any())
            {
                Console.WriteLine("      There is no In-Sync-Replica broker for the partition");
            }
            else
            {
                string isrs = string.Join("; ", partition.ISR);
                Console.WriteLine($"      The In-Sync-Replica brokers are: {isrs}");
            }

            if (!partition.Replicas.Any())
            {
                Console.WriteLine("      There is no Replica broker for the partition");
            }
            else
            {
                string replicas = string.Join("; ", partition.Replicas);
                Console.WriteLine($"      The Replica brokers are: {replicas}");
            }
        }

        Console.WriteLine($"  Is internal: {topic.IsInternal}");
        if (includeAuthorizedOperations)
        {
            string operations = string.Join(" ", topic.AuthorizedOperations);
            Console.WriteLine($"  Authorized operations: {operations}");
        }
    }
}


static async Task DescribeTopicsAsync(string bootstrapServers)
{
    var topicNames = new List<string>
    {
        "GidUralTest",
        "test-topic"
    };
    
    var timeout = TimeSpan.FromSeconds(30);
    var config = new AdminClientConfig
    {
        BootstrapServers = bootstrapServers,
    };
    
    using (var adminClient = new AdminClientBuilder(config).Build())
    {
        try
        {
            var descResult = await adminClient.DescribeTopicsAsync(TopicCollection.OfTopicNames(topicNames), new DescribeTopicsOptions() {RequestTimeout = timeout, IncludeAuthorizedOperations = false});
            PrintTopicDescriptions(descResult.TopicDescriptions, false);
        }
        catch (DescribeTopicsException e)
        {
            // At least one TopicDescription will have an error.
            PrintTopicDescriptions(e.Results.TopicDescriptions, false);
        }
        catch (KafkaException e)
        {
            Console.WriteLine($"An error occurred describing topics: {e}");
            Environment.ExitCode = 1;
        }
    }
}


Console.WriteLine($"librdkafka Version: {Library.VersionString} ({Library.Version:X})");
Console.WriteLine($"Debug Contexts: {string.Join(", ", Library.DebugContexts)}");


var brokerEndPoint = "localhost:29092";
//ListConsumerGroups(brokerEndPoint);
PrintMetadata(brokerEndPoint);
//await DescribeTopicsAsync(brokerEndPoint);