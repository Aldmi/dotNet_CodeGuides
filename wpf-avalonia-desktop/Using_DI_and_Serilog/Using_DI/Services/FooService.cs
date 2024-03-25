using System;
using Serilog;
using Serilog.Context;

namespace Using_DI.Services;

public class FooService : IFooService, IDisposable
{
    private int _counter;
    
    public FooService()
    {
        //Для задания специфического контекста (А=1) для группы событий логирования.
        using (LogContext.PushProperty("A", 1))
        {
            Log.Information("Carries property A = 1");
            Log.Information("ctor Repository");
        }
        
        //Для указания имени класса в логе.
        var myLog = Log.ForContext<FooService>();
        myLog.Information("Hello!");
        
        ObjGuid = Guid.NewGuid();
    }

    public Guid ObjGuid { get; init; }
    
    public int GetCounterValue() => ++_counter;

    public void Dispose()
    {
        Log.Warning("Dispose FooService ...");
    }
}