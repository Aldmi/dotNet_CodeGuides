using App.Core;
using Infrastructure.SignalRhub;
using Infrastructure.SignalRhub.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalRService(builder.Configuration);
builder.Services.AddAppCore(builder.Configuration);

//CORS- для Angular client  запущенного на другом хосте (localhost:4200).
builder.Services.AddCors(options =>
{
    //WithOrigins и AllowCredentials - ОБЯЗАТЕЛЬНО, для SignalR клиента с другого хоста.
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        //.WithOrigins("http://localhost:4200")
        .SetIsOriginAllowed((host) => true) //Ручная проверка данных хоста (можно заменить WithOrigins, если не указывать разрещенные адреса)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials() //разрешается принимать идентификационные данные от клиента
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapSignalRHub(builder.Configuration);

//Сервер - инициатор посылки запросов.
app.MapGet("Receive", async (IHubContext<ChatHub> hubContext) =>
{
    // await hubContext.Clients.All.SendAsync("Receive from GET Receive endpoints", "Alex", $"Message - {DateTime.Now.ToLongTimeString()}");
    await hubContext.Clients.All.SendAsync("Receive", new Persone("Alex", 20, DateTime.Now));
});

app.Run();