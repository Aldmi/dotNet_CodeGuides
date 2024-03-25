using System.Globalization;

namespace FactoryPatternUsingDi.Samples;

public interface ISample1 : IDisposable
{
    string CurrentDateTime { get; set; }
}

public class Sample1 : ISample1
{
    public string CurrentDateTime { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

    public Sample1()
    {
        Console.WriteLine("ctor Sample1 >>>>>>>>>>");
    }
    
    public void Dispose()
    {
       Console.WriteLine("Dispose Sample1 <<<<<<<<<");
    }
}