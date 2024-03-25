using FactoryPatternUsingDi.Samples;

namespace FactoryPatternUsingDi.Factories;


/// <summary>
/// реализация как у AbstractFactory, только для конкртеного типа IUserData.
/// При создании объекта нужно инизиализтровать некоторые поля IUserDataFactory.Create(string name)
/// </summary>
public static class GenerateClassWithDataFactoryExtension
{
    public static IServiceCollection AddGenerateClassWithDataFactory(this IServiceCollection services)
    {
        // IoC ответчает только за время жизни самой фабрики (name) => new UserData(name). для очиски IUserData объекта нужно вручную вызывать Dispose() когда нужно. 
        services.AddSingleton<Func<string, IUserData>>(_ => (name) => new UserData(name)); 
        services.AddSingleton<IUserDataFactory, UserDataFactory>();
        return services;
    }
}

public interface IUserDataFactory
{
    IUserData Create(string name);
}

public class UserDataFactory : IUserDataFactory
{
    private readonly Func<string, IUserData> _factory;

    public UserDataFactory(Func<string, IUserData> factory)
    {
        _factory = factory;
    }

    public IUserData Create(string name)
    {
        var output = _factory(name);
        return output;
    }
}