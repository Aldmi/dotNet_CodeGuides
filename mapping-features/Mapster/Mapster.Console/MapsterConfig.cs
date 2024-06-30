using Application.Features.GetProduct;
using Domain.Entities;

namespace Mapster.Console;

public class MapsterConfig
{
    public MapsterConfig()
    {
        TypeAdapterConfig<GetProductDto, Product>.NewConfig()
            .ConstructUsing(dto => new Product(
                dto.ProductId,
                dto.Name,
                dto.Count,
                dto.Cost,
                dto.CreateTime
                ));
        
        TypeAdapterConfig<Product, GetProductDto>.NewConfig()
            .Map(dest => dest.Name, src => src.Name + " !!!!!");
    }
}