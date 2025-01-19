using Microsoft.AspNetCore.Mvc;
using WebApi.Database;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/getProducts", async (
        [FromQuery]string? sortOrder,
        [FromQuery]string? searchPattern,
        [FromQuery]int pageSize,
        [FromQuery]int pageNumber,
        [FromServices] ApplicationDbContext dbContext) =>
    {
        //await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        searchPattern += "%";
        var nextPage = await dbContext.Products.GetNextPage(
            sortOrder,
            searchPattern,
            pageSize,
            pageNumber
           );

        return new
        {
            items= nextPage.ToList(),
            pageIndex= nextPage.PageIndex,
            totalPages = nextPage.TotalPages,
            hasNextPage = nextPage.HasNextPage,
            hasPreviousPage = nextPage.HasPreviousPage
        };
    })
    .WithName("GetProducts");

app.Run();

