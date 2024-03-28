using FactoryPatternUsingDi.Samples;

namespace FactoryPatternUsingDi.Factories;


//builder.Services.AddTransient<ISample1, Sample1>();
//Варианты регистрации фабрик:
//Вариант 1. Простая фабрика Func<ISample1>, которую можно внедрить везед где нужно в runtime создавать объекты.
//builder.Services.AddSingleton<Func<ISample1>>(serviceProvider => () => serviceProvider.GetRequiredService<ISample1>());


public static class AbstractFactoryExtensions
{
    public static IServiceCollection AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection services)
    where TInterface : class
    where TImplementation : class, TInterface
    {
        services.AddTransient<TInterface, TImplementation>();
        services.AddScoped<Func<TInterface>>(s=> ()=> s.GetRequiredService<TInterface>()); //Не Singleton - чтобы не удерживать созданный экземпляр TInterface в памяти при выходе за scope. (Иначе не будет вызван метод Dispose.)
        services.AddScoped<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();   //Не Singleton - чтобы не удерживать созданный экземпляр TInterface в памяти при выходе за scope. (Иначе не будет вызван метод Dispose.)
        return services;
    }
}


public class AbstractFactory<T> : IAbstractFactory<T>
{
    private readonly Func<T> _factory;

    public AbstractFactory(Func<T> factory)
    {
        _factory = factory;
    }

    public T Create()
    {
        return _factory();
    }
}


public interface IAbstractFactory<out T>
{
    T Create();
}