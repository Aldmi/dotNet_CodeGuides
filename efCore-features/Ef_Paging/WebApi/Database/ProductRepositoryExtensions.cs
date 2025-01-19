using Ef_Paging.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database;

public static class ProductRepositoryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="products">запрос на получение продуктов</param>
    /// <param name="sortOrder">Сортировка</param>
    /// <param name="searchPattern">Строка поиска для SQL LINK выражения</param>
    /// <param name="pageSize">кол-во элементов на странице</param>
    /// <param name="pageNumber">номер страницы</param>
    /// <returns>Страница с данными</returns>
    public static async Task<PaginatedList<Product>> GetNextPage(this IQueryable<Product> products,
        string? sortOrder,
        string? searchPattern,
        int pageSize,
        int? pageNumber)
    {
        if (!string.IsNullOrEmpty(searchPattern))
        {
            products = products.Where(product => EF.Functions.Like(product.Name, searchPattern));
        }

        products = sortOrder switch
        {
            "name_desc" => products.OrderByDescending(s => s.Name),
            "Date" => products.OrderBy(s => s.CreatedAtUtc),
            "date_desc" => products.OrderByDescending(s => s.CreatedAtUtc),
            _ => products.OrderBy(s => s.Name)
        };

        return await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize);
    }
}