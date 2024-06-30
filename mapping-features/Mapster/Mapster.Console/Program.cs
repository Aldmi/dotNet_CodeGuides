using Application.Features.GetProduct;
using Domain.Entities;
using Mapster;
using Mapster.Console;

//TypeAdapterConfig.GlobalSettings.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);

//TypeAdapterConfig.GlobalSettings.Default.EnableNonPublicMembers(true);

//валидация глобальной конфигурации
//TypeAdapterConfig.GlobalSettings.Compile();


var mapsterConfig = new MapsterConfig();


// //Validate a specific config
// var config = TypeAdapterConfig<Product, GetProductDto>.NewConfig();
// config.Compile();


var product = new Product(
    Guid.NewGuid(),
    "Meat",
    6520,
    100,
    DateTime.UtcNow
);

var dto = product.Adapt<GetProductDto>();

var productFromDto = dto.Adapt<Product>();


Console.WriteLine("Hello, World!");