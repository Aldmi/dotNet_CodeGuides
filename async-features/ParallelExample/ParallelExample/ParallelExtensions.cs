using System.Collections.Concurrent;

namespace ParallelExample;

public static class ParallelExtensions
{
    public static Task ParallelForEachAsync<T>(
        IEnumerable<T> source,
        int degreeOfParalelizm,
        Func<T, Task> body)
    {
        var tasks = Partitioner
            .Create(source)
            .GetPartitions(degreeOfParalelizm)
            .AsParallel()
            .Select(enumerator => AwaitPartition(enumerator));
        
        return Task.WhenAll(tasks);

        async Task AwaitPartition(IEnumerator<T> partition)
        {
            using (partition)
            {
                while (partition.MoveNext())
                {
                    await body(partition.Current);
                }
            }
        }
    }
}