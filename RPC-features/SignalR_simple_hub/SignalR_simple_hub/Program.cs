using Microsoft.AspNetCore.SignalR;
using SignalR_simple_hub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR().AddHubOptions<ChatHub>(options =>  
{
    //options.EnableDetailedErrors = true;
    //options.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(10); //30 default
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
             .WithOrigins("http://localhost:4200")
            //.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
         .AllowCredentials()
    );
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapHub<ChatHub>("/chat");

//Сервер - инициатор посылки запросов.
app.MapGet("Receive", async (IHubContext<ChatHub> hubContext) =>
{
    // await hubContext.Clients.All.SendAsync("Receive from GET Receive endpoints", "Alex", $"Message - {DateTime.Now.ToLongTimeString()}");
    await hubContext.Clients.All.SendAsync("Receive", new Persone("Alex", 20, DateTime.Now));
});

app.Run();
