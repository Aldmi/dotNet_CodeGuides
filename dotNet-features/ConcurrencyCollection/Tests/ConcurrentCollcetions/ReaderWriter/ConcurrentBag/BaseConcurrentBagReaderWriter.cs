using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tests.ConcurrentCollcetions.ReaderWriter.ConcurrentBag;

public abstract class BaseConcurrentBagReaderWriter
{
    protected readonly ConcurrentBag<int> NumbersBag = new();
    public IEnumerable<int>  BagOfNumbers => NumbersBag;
    
    public long Execute(ThreadExecutionConfiguration config)
    {
        var tasks = new List<Task>(config.ReaderThreadsCount + config.WriterThreadsCount);
        for (var cnt = 0; cnt < config.ReaderThreadsCount; cnt++)
        {
            var readTask = new Task(() => 
                ReadListCount(config.ReaderExecutionsCount, config.ReaderExecutionDelay, config.ReaderEnumerateAll));
            tasks.Add(readTask);
        }
        for (var cnt = 0; cnt < config.WriterThreadsCount; cnt++)
        {
            var writeTask = new Task(() => 
                AddNumbersToList(config.WriterExecutionsCount, config.WriterExecutionDelay));
            tasks.Add(writeTask);
        }
        tasks = tasks.OrderBy(x => Random.Shared.Next()).ToList(); 
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (var task in tasks)
        {
            task.Start();
        }
        Task.WhenAll(tasks).Wait();
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
    
    protected abstract void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay);
    protected abstract void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll);
}