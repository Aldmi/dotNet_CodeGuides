using Microsoft.Extensions.Options;
using WorkWithConfig;
using WorkWithConfig.ConfigureOptions;

var builder = WebApplication.CreateBuilder(args);

//Вариант 1.  байндинг через Ioptions
//уровни вложенности в enviroment переаменных отделяются __  
builder.Configuration.AddEnvironmentVariables(prefix: "APP_NAME__"); //APP_NAME__ - префикс который игноритруется при чтении по имени.
builder.Services.ConfigureOptions<ApplicationOptionsSetup>();

//Вариант 2. При запуске приложения создавать эеземпляр настроек и регистрировать его в DI
var appOptions= builder.Services.RegisterApplicationOptions(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


app.MapGet("options", (IOptions<ApplicationOptions> options, ApplicationOptions appOptions) =>
{
    var response = new
    {
        MyDbOption_Connection= options.Value.MyDbOption.Connection,
        MyDbOption_Config_param1= options.Value.MyDbOption.Config.param1,
        MessageBrokerOption_Url= options.Value.MessageBrokerOption.Url,
        MessageBrokerOption_TopicName= options.Value.MessageBrokerOption.TopicName,
        
    };
    return Results.Ok(response);
});

app.Run();