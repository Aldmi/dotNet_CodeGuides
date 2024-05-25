using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHangfire(config =>
{
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage();
});

//сервер Hangfire - выполняет опрос хранилища на наличие заданий и ставит их на выполнение.
builder.Services.AddHangfireServer(x=>
    x.SchedulePollingInterval = TimeSpan.FromSeconds(1) //Интервал опроса сервера
    );


var app = builder.Build();

app.UseHangfireDashboard();

app.MapGet("/job", (IBackgroundJobClient jobClient) =>
{

    //Ставит задачу в очередь на выполне6ний
    jobClient.Enqueue(() => Console.WriteLine("Hello from BH"));
    return Results.Ok("Job..");
});

app.Run();

