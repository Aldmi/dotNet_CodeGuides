using System.Globalization;

namespace FactoryPatternUsingDi.Samples;

public interface ISample2 : IDisposable
{
    int RandomValue { get;  }
}

public class Sample2 : ISample2
{
    public int RandomValue { get; }

    public Sample2()
    {
     
        Console.WriteLine("ctor Sample2 >>>>>>>>>>");
        RandomValue = Random.Shared.Next(1, 100);
    }
    
    public void Dispose()
    {
        Console.WriteLine("Dispose Sample2 <<<<<<<<<");
    }
}