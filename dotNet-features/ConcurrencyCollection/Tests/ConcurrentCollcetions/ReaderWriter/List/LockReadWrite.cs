using System.Security.Cryptography;

namespace Tests.ConcurrentCollcetions.ReaderWriter.List;

public class LockReadWrite : BaseReaderWriter
{
    private static readonly object LockObject = new();

    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        lock (LockObject)
        {
            for (var cnt = 0; cnt < writerExecutionsCount; cnt++)
            {
                NumbersList.Add(cnt);
                Thread.SpinWait(writerExecutionDelay);
            }
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
    {
        lock (LockObject)
        {
            for (var cnt = 0; cnt < readerExecutionsCount; cnt++)
            {
                if (NumbersList.Count > 0)
                {
                    _ = NumbersList[Random.Shared.Next(0, NumbersList.Count)];
                    if (readerEnumerareAll)
                    {
                        foreach (var item in NumbersList)
                        {
                            var i=item;
                        }
                    }
                }
                Thread.SpinWait(readerExecutionDelay);
            }
        }
    }
}