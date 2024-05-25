using Hangfire;
using Hangfire_InPgSqlStorage.Jobs;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<SimpleJob>();

var hangfireConnectionStr = builder.Configuration.GetConnectionString("HangfireSqlPg");
builder.Services.AddHangfire(config =>
{
    config
         //Serialize Методы задают правила по сериализации методов которые помешаются в очерди на выполнение. 
         // При помещении в очередь происходит сохранение всей информации про метод с параметрами в хранилище.
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(hangfireConnectionStr));
});

//сервер Hangfire - выполняет опрос хранилища на наличие заданий и ставит их на выполнение.
builder.Services.AddHangfireServer(x=>
        x.SchedulePollingInterval = TimeSpan.FromSeconds(2) //Интервал опроса сервера
);


var app = builder.Build();

app.UseHangfireDashboard();

app.MapGet("/job_Enqueue", (IBackgroundJobClient jobClient) =>
{
    //Ставит задачу в очередь на выполнение
    jobClient.Enqueue(() => Console.WriteLine("Hello from job_Enqueue"));
    return Results.Ok("job_Enqueue..");
});

app.MapGet("/job_Schedule", (IBackgroundJobClient jobClient) =>
{
    //Ставит задачу в очередь на выполнение через указанное время задержки
    jobClient.Schedule(
        () => Console.WriteLine("Hello from job_Schedule"),
        TimeSpan.FromSeconds(5)
        );
    return Results.Ok("job_Schedule..");
});

app.MapGet("/job_recurring", (IRecurringJobManager jobManager) =>
{
    //Поставить повторяюшуюся задачу на выполнените
    jobManager.AddOrUpdate(
        "every5sec",
        () => Console.WriteLine("Hello from job_recurring"),
        "*/5 * * * *"
    );
    return Results.Ok("job_recurring..");
});

app.MapGet("/job_recurring_2", (IRecurringJobManager jobManager) =>
{
    //Поставить повторяюшуюся задачу на выполнените, с использованием DI для внедрения класса с задачей.
    jobManager.AddOrUpdate<SimpleJob>(
        "every5sec_Ver2",
        obj =>obj.DoSomething(),
        "*/5 * * * *" );
    return Results.Ok("job_recurring_2..");
});

app.Run();