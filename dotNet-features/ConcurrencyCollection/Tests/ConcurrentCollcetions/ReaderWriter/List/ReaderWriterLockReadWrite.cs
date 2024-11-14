namespace Tests.ConcurrentCollcetions.ReaderWriter.List;

public class ReaderWriterLockReadWrite : BaseReaderWriter
{
    private static readonly ReaderWriterLock ReaderWriterLock = new();

    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        ReaderWriterLock.AcquireWriterLock(Timeout.InfiniteTimeSpan);

        try
        {
            for (var cnt = 0; cnt < writerExecutionsCount; cnt++)
            {
                NumbersList.Add(cnt);
                Thread.SpinWait(writerExecutionDelay);
            }
        }
        finally
        {
            ReaderWriterLock.ReleaseWriterLock();
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
    {
        ReaderWriterLock.AcquireReaderLock(Timeout.InfiniteTimeSpan);

        try
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
        finally
        {
            ReaderWriterLock.ReleaseReaderLock();
        }
    }
}