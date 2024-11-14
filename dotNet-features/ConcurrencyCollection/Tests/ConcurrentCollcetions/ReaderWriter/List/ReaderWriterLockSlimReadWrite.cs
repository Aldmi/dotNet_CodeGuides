namespace Tests.ConcurrentCollcetions.ReaderWriter.List;

public class ReaderWriterLockSlimReadWrite : BaseReaderWriter
{
    private static readonly ReaderWriterLockSlim ReaderWriterLockSlim = new();

    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        ReaderWriterLockSlim.EnterWriteLock();

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
            ReaderWriterLockSlim.ExitWriteLock();
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
    {
        ReaderWriterLockSlim.EnterReadLock();

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
            ReaderWriterLockSlim.ExitReadLock();
        }
    }

    public void ReadOrAdd(int writerExecutionDelay)
    {
        var random = Random.Shared.Next(0, NumbersList.Count);

        ReaderWriterLockSlim.EnterUpgradeableReadLock();

        try
        {
            if (!NumbersList.Contains(random))
            {
                ReaderWriterLockSlim.EnterWriteLock();

                try
                {
                    NumbersList.Add(random);
                    Thread.SpinWait(writerExecutionDelay);
                }
                finally
                {
                    ReaderWriterLockSlim.ExitWriteLock();
                }
            }
        }
        finally
        {
            ReaderWriterLockSlim.ExitUpgradeableReadLock();
        }
    }
}