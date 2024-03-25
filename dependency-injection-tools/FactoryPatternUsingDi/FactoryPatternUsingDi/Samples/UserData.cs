namespace FactoryPatternUsingDi.Samples;

public interface IUserData : IDisposable
{
    string? Name { get; set; }
}

public class UserData : IUserData
{
    public UserData(string name)
    {
        Console.WriteLine("ctor UserData >>>>>>>>>>");
        Name = name;
    }
    
    public string? Name { get; set; }
    
    public void Dispose()
    {
        Console.WriteLine("Dispose UserData <<<<<<<<<");
    }
}