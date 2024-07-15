using Hangfire;
using Hangfire.PostgreSql;
using LongRunning.Api.Hubs;
using LongRunning.Api.Jobs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddHangfire(configuration => configuration.UsePostgreSqlStorage(
    options => options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("LongRunningTasksDb"))));

builder.Services.AddHangfireServer(option => option.SchedulePollingInterval = TimeSpan.FromSeconds(1));

builder.Services.AddTransient<LongRunningJob>();
builder.Services.AddTransient<LongRunningJobWithNotification>();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(policyBuilder => policyBuilder.AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("*"));

app.UseHangfireDashboard();

app.MapHub<NotificationHub>("notifications");


//Endpoints------------------------------------------------------------------------------------------------

//v1--------------------------------------------------------
app.MapGet("reports/v1", async (ILogger<Program> logger) =>
{
    logger.LogInformation("Start background job");
    await Task.Delay(TimeSpan.FromSeconds(3));
    logger.LogInformation("Completed background job");
    return "Completed";
});

//v2--------------------------------------------------------
app.MapPost("reports/v2", (IBackgroundJobClient backgroundJobClient) =>
{
    string jobId= backgroundJobClient.Enqueue<LongRunningJob>(job => job.ExecuteAsync(CancellationToken.None));
    //возвращает ответ в котором указывается jobId. В Headers устанавливается поле "Location" со значением http://localhost:5101/jobs/12
    //Это поле использует фронтенд для GET запроса. 
    return Results.AcceptedAtRoute("JobDetails", new { jobId }, jobId);
});

app.MapGet("jobs/{jobId}", (string jobId) =>
{
    var jobDetails = JobStorage.Current.GetMonitoringApi().JobDetails(jobId);
    return jobDetails.History.OrderByDescending(h => h.CreatedAt).First().StateName;
})
.WithName("JobDetails");

//v3--------------------------------------------------------
app.MapPost("reports/v3", (IBackgroundJobClient backgroundJobClient, IHubContext<NotificationHub> hubContext) =>
{
    string jobId= backgroundJobClient.Enqueue<LongRunningJobWithNotification>(job => job.ExecuteAsync(CancellationToken.None));

    hubContext.Clients.All.SendAsync("ReciveNotification", $"Started processing job woth ID: {jobId}");
        
    return Results.AcceptedAtRoute("JobDetails", new { jobId }, jobId);
});


app.Run();