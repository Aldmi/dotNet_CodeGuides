using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Models;
using WebApi.Domain.Entities.Specifications;
using WebApi.Infrastructure.Persistence.Pg;

namespace WebApi.Application.Core.Features.Products.GetProductFeature;

public class GetProductService(ApplicationDbContext applicationDbContext)
{
    /// <summary>
    /// Вернуть весь список продуктов без фильтрации
    /// </summary>
    public async Task<List<Product>> GetAllProducts()
    {
        var productList = await applicationDbContext.Products
            .AsNoTracking()
            .ToListAsync();

        return productList;
    }


    /// <summary>
    /// Вернуть только доступные продукты. применив спецификацию IsAvailable
    /// </summary>
    public async Task<List<Product>> GetAvailableProducts()
    {
        /*
          SELECT p."Id", p."IsSaleEnabled", p."Name", p."Price", p."StockCount"
          FROM "Products" AS p
          WHERE p."IsSaleEnabled" AND p."StockCount" > 0
         */
        var productList = await applicationDbContext.Products
            .Where(p => p.IsAvailable())
            .AsNoTracking()
            .ToListAsync();

        return productList;
    }


    /// <summary>
    /// Можно объединять делегаты спецификации и обычные выражения
    /// </summary>
    public async Task<List<Product>> GetAvailableAndExpansiveProducts(decimal maxPrice)
    {
        /*
          SELECT p."Id", p."IsSaleEnabled", p."Name", p."Price", p."StockCount"
          FROM "Products" AS p
          WHERE p."IsSaleEnabled" AND p."StockCount" > 0 AND p."Price" >= @__maxPrice_0
         */
        var productList = await applicationDbContext.Products
            .Where(p => p.IsAvailable() && p.Price >= maxPrice)
            .AsNoTracking()
            .ToListAsync();

        return productList;
    }


    /// <summary>
    /// Можно обьединять спецификации по булевым условиям
    /// </summary>
    public async Task<List<Product>> GetAvailableAndNameStartWithProducts(string nameStartWith)
    {
        /*

         */
        var productList = await applicationDbContext.Products
            .Where(p => p.IsAvailable() || p.NameStartWith(nameStartWith))
            .AsNoTracking()
            .ToListAsync();

        return productList;
    }

    
    /// <summary>
    /// 
    /// </summary>
    public async Task<List<Product>> GetAvailableProductsInCategoryName(string categoryName)
    {
        /*
          SELECT p."Id", p."CategoryId", p."IsSaleEnabled", p."Name", p."Price", p."StockCount", c."Id", c."Name"
          FROM "Products" AS p
          LEFT JOIN "Categories" AS c ON p."CategoryId" = c."Id"
          WHERE p."IsSaleEnabled" AND p."StockCount" > 0 AND c."Name" = @__categoryName_0
        */
        var productList = await applicationDbContext.Products
            .Include(p=>p.Category)
            .Where(p => p.IsAvailable() && p.Category.Name == categoryName)
            .AsNoTracking()
            .ToListAsync();

        return productList;
    }

    /// <summary>
    /// Вложенные сущности поиск в категориях по продуктам.
    /// Использовангие спецификации для продуктов.
    /// </summary>
    public async Task<List<Category>> GetCategorysWithAvailableProducts()
    {
        /*
          SELECT c."Id", c."Name"
          FROM "Categories" AS c
          WHERE EXISTS (
          SELECT 1
          FROM "Products" AS p
          WHERE c."Id" = p."CategoryId" AND p."IsSaleEnabled" AND p."StockCount" > 0)
         */
        var categoriesList = await applicationDbContext.Categories
            .Where(c => c.Products.Any(p => p.IsAvailable()))
            .AsNoTracking()
            .ToListAsync();

        return categoriesList;
    }
}