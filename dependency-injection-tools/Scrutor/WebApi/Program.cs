using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Domain;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Scan(selector => selector.FromAssemblyOf<Program>()
	.AddClasses(classSelector => classSelector.AssignableTo<IUserService>())
	.AsImplementedInterfaces()
	.WithScopedLifetime()
	
	.AddClasses(classSelector => classSelector.AssignableTo(typeof(IRepository<>)))
	.AsImplementedInterfaces()
	.WithScopedLifetime()
);

//Регистрируем декораторы, вызываться они будут в порядке регистрации

//Можно декорировать конкретные реализации
//builder.Services.Decorate<IRepository<User>, RepositoryLoggerDecorator<User>>();

//или декорировать дженерики
builder.Services.Decorate(typeof(IRepository<>), typeof(RepositoryLoggerDecorator<>));
builder.Services.Decorate(typeof(IRepository<>), typeof(RepositoryValidatorDecorator<>));

var app = builder.Build();

app.MapGet("/weatherforecast", (
	IUserService userService,
	IRepository<User> userRepository //внедрится RepositoryLoggerDecorator<User> в котором будет RepositoryValidatorDecorator в котором будет UserRepository
	) =>
{
	return Results.Ok("Weather forecast");
});

app.Run();

