using Application;
using Application.DataAccess.Abstract;
using Application.Features.GetProduct;
using Domain.Entities;
using Infrastucture.DataAccess.Pg;
using Mapster_WebApi;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterMapsterConfiguration();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/getNewProduct", (
        IMapper mapper,
        IDbContext dbContext) =>
    {
        var product = new Product(
            Guid.NewGuid(),
            "Meat",
            6520,
            100,
            DateTime.UtcNow
        );

        var productDto=mapper.Map<GetProductDto>(product);
        
        var productModel= mapper.Map<Product>(productDto);
        
        return productModel;
    })
    .WithOpenApi();


app.MapGet("/getProductsFromDb", async (
        GetProductReadService getProductReadService
        ) =>
        {
            var listProductReadDto = await getProductReadService.GetAllProducts();
            return listProductReadDto;
        })
    .WithOpenApi();

//можно отдавать IQueryable<GetProductDto> из endPoint
app.MapGet("/getProductsFromDbQuery",  (
        GetProductReadService getProductReadService
    ) =>
    {
        var query = getProductReadService.GetAllProductsQuery();
        return query;
    })
    .WithOpenApi();

app.Run();
