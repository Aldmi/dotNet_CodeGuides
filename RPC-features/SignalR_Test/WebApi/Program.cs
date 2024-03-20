using App.Core;
using Infrastructure.SignalRhub;
using Infrastructure.SignalRhub.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalRService(builder.Configuration);
builder.Services.AddAppCore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapSignalRHub(builder.Configuration);

//Сервер - инициатор посылки запросов.
app.MapGet("Receive", async (IHubContext<ChatHub> hubContext) =>
{
   // await hubContext.Clients.All.SendAsync("Receive from GET Receive endpoints", "Alex", $"Message - {DateTime.Now.ToLongTimeString()}");
   await hubContext.Clients.All.SendAsync("Receive", new Persone("Alex", 20, DateTime.Now));
});

app.Run();