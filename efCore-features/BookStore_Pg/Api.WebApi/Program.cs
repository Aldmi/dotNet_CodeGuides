using Api.WebApi.Options.Application;
using Infrastructure.Persistance.Pg;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

//Bind Configuration to ApplicationOptions----------------------------------------------------------------
var appOptions= builder.Services.AddApplicationOptions(builder.Configuration);
var result= appOptions.Validate();
if (result.IsFailure) {
    logger.LogError("{ApplicationOptions}", result.Error);
    Console.ReadKey();
    return -1;
}
else {
    logger.LogInformation("{ApplicationOptions}", appOptions.ToString());
}

// Add services to the container.
builder.Services.AddPersistence(appOptions.PgDbOption, LogPersistence);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
return 0;


void LogPersistence(string message)
{
    logger.LogInformation(message);
}