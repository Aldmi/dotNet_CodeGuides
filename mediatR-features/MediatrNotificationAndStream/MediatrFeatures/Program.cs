using MediatR;
using MediatrFeatures;
using MediatrFeatures.Notifications;
using MediatrFeatures.Request;
using MediatrFeatures.Stream;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GetWeatherService>();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/request/{city}", async ([FromRoute] string city, IMediator mediator) =>
{
    var request = new GetWeatherRequest {City = city};
    var response = await mediator.Send(request);
    return Results.Ok(response);
});


app.MapPost("/notification", async ([FromBody] WeatherWarning warning, IMediator mediator) =>
{
    var notification = new WetherWarningNotification(warning.Message);
    await mediator.Publish(notification);
    return Results.Ok();
}); 


//CMD: curl http://localhost:5037/stream/erer
app.MapGet("/stream/{city}", ([FromRoute] string city, IMediator mediator, CancellationToken ct) =>
{
    var streamRequest = new WeatherUpdateStreamRequest(city);
    return Results.Ok(mediator.CreateStream(streamRequest, ct)); //Отдает IAsyncEnumerable<WeatherForecast>, поэтому клиент должен поддержтвать получения потоковых данных
}); 

app.Run();