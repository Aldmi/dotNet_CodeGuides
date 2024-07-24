using Microsoft.EntityFrameworkCore;
using WebApi.Application.Core.Features.Products.GetProductFeature;
using WebApi.Domain;
using WebApi.Domain.Entityes;
using WebApi.Infrastructure.Persistence.Pg;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options
        .UseNpgsql(connection)
        .UseProjectables();    //указываем бибюлиотеку для генерации Expression 
});

builder.Services.AddScoped<GetProductService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/getAllProducts", async (GetProductService productService) =>
    {
        var products = await productService.GetAllProducts();
        return products;
    })
    .WithName("getAllProducts")
    .WithOpenApi();


app.MapGet("/addProduct", async (ApplicationDbContext dbContext) =>
    {
        var newProduct = new Product()
        {
            Id = Guid.NewGuid(), //Можно указывать или не указывать Guid.
            Name = "Apple",
            Price = 98,
            IsSaleEnabled = true
        };
        var product = dbContext.Add(newProduct);
        await dbContext.SaveChangesAsync();
        return product.Entity;
    })
    .WithName("addProduct")
    .WithOpenApi();

app.Run();
