using Application.DataAccess.Abstract;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GetProduct;

/// <summary>
/// ReadModel для Product
/// </summary>
/// <param name="dbContext"></param>
public class GetProductReadService(IDbContext dbContext, IMapper mapper)
{
    /// <summary>
    /// Mapster делает проекцию объектов (SELECT) загружая только нужные данные
    /// </summary>
    public async Task<List<GetProductDto>> GetAllProducts()
    {
       var query= mapper.From(dbContext.Products).ProjectToType<GetProductDto>();
       var productsDto= await query.ToListAsync();
       return productsDto;
    }
    
    
    public IQueryable<GetProductDto> GetAllProductsQuery()
    {
        var query= mapper.From(dbContext.Products).ProjectToType<GetProductDto>();
        return query;
    }
}