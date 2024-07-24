using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
using WebApi.Domain.Entityes;
using WebApi.Domain.Specifications;
using WebApi.Infrastructure.Persistence.Pg;

namespace WebApi.Application.Core.Features.Products.GetProductFeature;

public class GetProductService(ApplicationDbContext applicationDbContext)
{
    public async Task<List<Product>> GetAllProducts()
   {
       // var productList= await applicationDbContext.Products
       //     .Where(p=>p.Price > 0 && p.StockCount > 0)
       //     .AsNoTracking()
       //     .ToListAsync();
       
       
       /*
         SELECT p."Id", p."IsSaleEnabled", p."Name", p."Price", p."StockCount"
         FROM "Products" AS p
         WHERE p."IsSaleEnabled" AND p."StockCount" > 0
        */
       var productList= await applicationDbContext.Products
           .Where(p=>p.IsAvailable())
           .AsNoTracking()
           .ToListAsync();
       
       return productList;
   }
    
}