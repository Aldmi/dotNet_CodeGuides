

namespace Tests.ConcurrentCollcetions.ReaderWriter.ConcurrentBag;

public class ConcurrentBagReadWrite : BaseConcurrentBagReaderWriter
{
    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        for (var cnt = 0; cnt < writerExecutionsCount; cnt++)
        {
            NumbersBag.Add(cnt);
            Thread.SpinWait(writerExecutionDelay);
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
    {
        for (var cnt = 0; cnt < readerExecutionsCount; cnt++)
        {
            if (!NumbersBag.IsEmpty)
            {
                //_ = NumbersBag.TryPeek(out var _);
                _ = NumbersBag.TryTake(out var _);
                if (readerEnumerareAll)
                {
                    foreach (var item in NumbersBag)
                    {
                        var i=item;
                    }
                }
            }

            Thread.SpinWait(readerExecutionDelay);
        }
    }
}