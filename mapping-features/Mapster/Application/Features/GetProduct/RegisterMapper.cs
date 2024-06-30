using Domain.Entities;
using Mapster;

namespace Application.Features.GetProduct;

public class RegisterMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetProductDto, Product>()
            .ConstructUsing(dto => new Product(
                dto.ProductId,
                dto.Name,
                dto.Count,
                dto.Cost,
                dto.CreateTime
            ))
            .RequireDestinationMemberSource(true);

        config.NewConfig<Product, GetProductDto>()
            .RequireDestinationMemberSource(true);
    }
}