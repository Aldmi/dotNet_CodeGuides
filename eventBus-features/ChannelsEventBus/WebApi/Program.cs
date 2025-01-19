using Application;
using Application.Features.RegisterUser.Command;
using Infrastructure.Common.ChannelEventBus;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddEventBus();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/addUser", (IMediator mediator) =>
{
    mediator.Send(new RegisterUserCommand(Guid.NewGuid(), "Alex"));
});


app.Run();
