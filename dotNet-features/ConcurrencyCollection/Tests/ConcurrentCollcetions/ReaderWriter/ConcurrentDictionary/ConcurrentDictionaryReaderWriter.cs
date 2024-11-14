namespace Tests.ConcurrentCollcetions.ReaderWriter.ConcurrentDictionary;

public class ConcurrentDictionaryReaderWriter : BaseConcurrentDictionaryReaderWriter
{
    protected override void AddNumbersToList(int writerExecutionsCount, int writerExecutionDelay)
    {
        for (var cnt = 0; cnt < writerExecutionsCount; cnt++)
        {
            NumbersDict.TryAdd(cnt, cnt);
            Thread.SpinWait(writerExecutionDelay);
        }
    }

    protected override void ReadListCount(int readerExecutionsCount, int readerExecutionDelay, bool readerEnumerareAll)
    {
        for (var cnt = 0; cnt < readerExecutionsCount; cnt++)
        {
            if (!NumbersDict.IsEmpty)
            {
                NumbersDict.TryGetValue(Random.Shared.Next(0, NumbersDict.Count), out _);
                if (readerEnumerareAll)
                {
                    foreach (var item in NumbersDict)
                    {
                        var i=item;
                    }
                }
            }
            Thread.SpinWait(readerExecutionDelay);
        }
    }
}