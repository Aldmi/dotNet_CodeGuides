using GreeterClient.WebAli.Services;
using GreeterClientApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<GreeterBackgroundService>();

//Создает один GrpcChannel (singleton) для каждого адреса
//Создает новый экземпляр клиента для каждой области (scoped по умолчанию)
//Управляет временем жизни соединений
builder.Services.AddGrpcClient<Greeter.GreeterClient>((provider, options) =>
{
	//из provider получить адрес сервера
	options.Address = new Uri("http://localhost:5098");
});

// Затем регистрируем ваш LangTranslator как синглтон
builder.Services.AddSingleton<LangTranslator>();

var app = builder.Build();

app.Run();

