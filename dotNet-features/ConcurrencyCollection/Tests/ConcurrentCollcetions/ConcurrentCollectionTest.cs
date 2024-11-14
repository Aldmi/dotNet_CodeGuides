using FluentAssertions;
using Tests.ConcurrentCollcetions.ReaderWriter;
using Tests.ConcurrentCollcetions.ReaderWriter.ConcurrentBag;
using Tests.ConcurrentCollcetions.ReaderWriter.ConcurrentDictionary;
using Tests.ConcurrentCollcetions.ReaderWriter.List;
using Xunit.Abstractions;

namespace Tests.ConcurrentCollcetions;

public class ConcurrentCollectionTest(ITestOutputHelper output)
{
    
    [Fact]
    public void Run_And_Compare_Lock_Primitives_ReaderEnumerateAll_false()
    {
        var config = new ThreadExecutionConfiguration
        {
            //TODO: почему когда много писателей, то Dict работает быстрее?
            ReaderThreadsCount = 20,
            WriterThreadsCount = 2,
            // ReaderThreadsCount = 2,
            // WriterThreadsCount = 20,
            ReaderExecutionDelay = 100,
            WriterExecutionDelay = 100,
            ReaderExecutionsCount = 10000,
            WriterExecutionsCount = 10000,
            ReaderEnumerateAll = false
        };
        
        var lockReadWrite = new LockReadWrite();
        output.WriteLine($"LockReadWrite execution time: {lockReadWrite.Execute(config)} milliseconds.");
        
        var readerWriterLockReadWrite = new ReaderWriterLockReadWrite();
        output.WriteLine($"ReaderWriterLock execution time: {readerWriterLockReadWrite.Execute(config)} milliseconds.");
        
        var readerWriterLockSlimReadWrite = new ReaderWriterLockSlimReadWrite();
        output.WriteLine($"ReaderWriterLockSlim execution time: " +
                         $"{readerWriterLockSlimReadWrite.Execute(config)} milliseconds.");
        
        var concurrentBagReadWrite = new ConcurrentBagReadWrite();
        output.WriteLine($"concurrentBagReadWrite execution time: " +
                         $"{concurrentBagReadWrite.Execute(config)} milliseconds.");
        
        var concurrentDictionaryReaderWriter = new ConcurrentDictionaryReaderWriter();
        output.WriteLine($"concurrentDictionaryReaderWriter execution time: " +
                         $"{concurrentDictionaryReaderWriter.Execute(config)} milliseconds.");
        
        // var readWrite = new ReadWrite();
        // output.WriteLine($"readWrite execution time: {readWrite.Execute(config)} milliseconds. Целостность данных не гарантирована при паралельной записиси для List");
    }
    
    
    
    [Fact]
    public void Run_And_Compare_Lock_Primitives_ReaderEnumerateAll_true()
    {
        var config = new ThreadExecutionConfiguration
        {
            ReaderThreadsCount = 20,
            WriterThreadsCount = 2,
            ReaderExecutionDelay = 100,
            WriterExecutionDelay = 100,
            ReaderExecutionsCount = 10000,
            WriterExecutionsCount = 10000,
            ReaderEnumerateAll = true
        };
        
        var lockReadWrite = new LockReadWrite();
        output.WriteLine($"LockReadWrite execution time: {lockReadWrite.Execute(config)} milliseconds.");
        
        var readerWriterLockReadWrite = new ReaderWriterLockReadWrite();
        output.WriteLine($"ReaderWriterLock execution time: {readerWriterLockReadWrite.Execute(config)} milliseconds.");
        
        var readerWriterLockSlimReadWrite = new ReaderWriterLockSlimReadWrite();
        output.WriteLine($"ReaderWriterLockSlim execution time: " +
                         $"{readerWriterLockSlimReadWrite.Execute(config)} milliseconds.");
        
        var concurrentBagReadWrite = new ConcurrentBagReadWrite();
        output.WriteLine($"concurrentBagReadWrite execution time: " +
                         $"{concurrentBagReadWrite.Execute(config)} milliseconds.");
        
        var concurrentDictionaryReaderWriter = new ConcurrentDictionaryReaderWriter();
        output.WriteLine($"concurrentDictionaryReaderWriter execution time: " +
                         $"{concurrentDictionaryReaderWriter.Execute(config)} milliseconds.");
        
        var readWrite = new ReadWrite();
        var act = () =>
        {
            readWrite.Execute(config);
        };
         act.Should().Throw<AggregateException>().Where(e=>e.Message.Contains("Collection was modified; enumeration operation may not execute."));
    }
}