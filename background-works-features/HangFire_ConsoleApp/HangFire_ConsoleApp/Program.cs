// See https://aka.ms/new-console-template for more information

using Hangfire;
using HangFire_ConsoleApp;
using Hangfire.MemoryStorage;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Hangfire Start !!!");


try
{
    GlobalConfiguration.Configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseColouredConsoleLogProvider()
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSerilogLogProvider()
        .UseMemoryStorage();
    
    BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

    RecurringJob.AddOrUpdate("easyjob", () => Console.Write("Easy!"), Cron.Minutely);
    
    RecurringJob.AddOrUpdate<EmailSender>("emailSender", (emailSender) => emailSender.Send(10, "new  Message"), Cron.Minutely);
    
    
    using (var server = new BackgroundJobServer())
    {
        Console.ReadLine();
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Something went wrong");
}
finally
{
    await Log.CloseAndFlushAsync();
}


