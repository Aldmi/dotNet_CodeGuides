namespace Tests.ConcurrentCollcetions.ReaderWriter.List;

public class ReadWrite : BaseReaderWriter
{
    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        for (var cnt = 0; cnt < writerExecutionsCount; cnt++)
        {
            NumbersList.Add(cnt);
            Thread.SpinWait(writerExecutionDelay);
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
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