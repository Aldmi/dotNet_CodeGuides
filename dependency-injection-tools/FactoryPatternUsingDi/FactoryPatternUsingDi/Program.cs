using FactoryPatternUsingDi.Components;
using FactoryPatternUsingDi.Factories;
using FactoryPatternUsingDi.Samples;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Варианты регистрации фабрик:
//-----------------------------------------------------
//Вариант 1. Простая фабрика Func<ISample1>, которую можно внедрить везед где нужно в runtime создавать объекты.
//builder.Services.AddTransient<ISample1, Sample1>();
//builder.Services.AddSingleton<Func<ISample1>>(serviceProvider => () => serviceProvider.GetRequiredService<ISample1>());

//-----------------------------------------------------
//Вариант 2. Мспользование AddAbstractFactory, которая скрывает явное использование Func<ISample1>
builder.Services.AddAbstractFactory<ISample1, Sample1>();
builder.Services.AddAbstractFactory<ISample2, Sample2>();

//Вариант 3. Пример фабрики для конкретного типа (не абстрактная)
builder.Services.AddGenerateClassWithDataFactory();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();