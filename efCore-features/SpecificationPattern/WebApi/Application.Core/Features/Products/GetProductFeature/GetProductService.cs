using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
using WebApi.Infrastructure.Persistence.Pg;

namespace WebApi.Application.Core.Features.Products.GetProductFeature;

public class GetProductService(ApplicationDbContext applicationDbContext)
{
    public async Task<List<Product>> GetAllProducts()
   {
       var productList= await applicationDbContext.Products.AsNoTracking().ToListAsync();
       return productList;
   }
    
}