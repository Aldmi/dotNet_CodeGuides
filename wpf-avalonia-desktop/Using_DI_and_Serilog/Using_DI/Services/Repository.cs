using Serilog;
using Serilog.Context;

namespace Using_DI.Services;

public class Repository : IRepository
{
    public Repository()
    {
        //Для задания специфического контекста (А=1) для группы событий логирования.
        using (LogContext.PushProperty("A", 1))
        {
            Log.Information("Carries property A = 1");
            Log.Information("ctor Repository");
        }
        
        //Для указания имени класса в логе.
        var myLog = Log.ForContext<Repository>();
        myLog.Information("Hello!");
    }
    
    public int GetId { get; } = 10;
}