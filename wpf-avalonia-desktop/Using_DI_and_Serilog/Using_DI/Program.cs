using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;

namespace Using_DI;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            //.Filter.ByIncludingOnly(Matching.WithProperty("Area", LogArea.Control))
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] |{SourceContext}|{Properties}{NewLine}{Message:lj}{NewLine}{Exception}")
            .WriteTo.File("./Logs/Log.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .Enrich.FromLogContext()
            .CreateLogger();
        
        Log.Information("Application starting ...");
        
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            //.LogToTrace()
            .UseReactiveUI();
}