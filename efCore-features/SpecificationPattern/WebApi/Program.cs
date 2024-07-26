using Microsoft.EntityFrameworkCore;
using WebApi.Application.Core.Features.Products.GetProductFeature;
using WebApi.Domain;
using WebApi.Domain.Entities.Models;
using WebApi.Infrastructure.Persistence.Pg;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options
        .UseNpgsql(connection)
        .UseProjectables(); //указываем бибюлиотеку для генерации Expression 
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


app.MapGet("/getAvailableProducts", async (GetProductService productService) =>
    {
        var products = await productService.GetAvailableProducts();
        return products;
    })
    .WithName("getAvailableProducts")
    .WithOpenApi();


app.MapGet("/getAvailableAndExpansiveProducts", async (GetProductService productService) =>
    {
        var products = await productService.GetAvailableAndExpansiveProducts(50);
        return products;
    })
    .WithName("getAvailableAndExpansiveProducts")
    .WithOpenApi();


app.MapGet("/getAvailableAndExpansiveProducts", async (GetProductService productService) =>
    {
        var products = await productService.GetAvailableAndNameStartWithProducts("Snick");
        return products;
    })
    .WithName("getAvailableAndExpansiveProducts")
    .WithOpenApi();


app.MapGet("/getAvailableProductsInCategoryName", async (GetProductService productService) =>
    {
        var products = await productService.GetAvailableProductsInCategoryName("Food");
        return products;
    })
    .WithName("getAvailableProductsInCategoryName")
    .WithOpenApi();


app.MapGet("/getCategorysWithAvailableProducts", async (GetProductService productService) =>
    {
        var categories = await productService.GetCategorysWithAvailableProducts();
        return categories;
    })
    .WithName("getCategorysWithAvailableProducts")
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