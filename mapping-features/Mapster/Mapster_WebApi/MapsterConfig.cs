using System.Reflection;
using Application.Features.GetProduct;
using Mapster;
using MapsterMapper;

namespace Mapster_WebApi;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        var config = GetMappingConfig();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    
    private static TypeAdapterConfig GetMappingConfig()
    {
        var config = new TypeAdapterConfig();
        
        //перечислить все Assembly, где укзанна IRegister конфигурация 
        config.Scan([
            Assembly.GetAssembly(typeof(GetProductDto))!
        ]);
        
        //Проверка конфигурации при запуске. (если в config стоит RequireDestinationMemberSource, то проверка целевого класса для маппинга выполняется)
        config.Compile(false);
        
        return config;
    }
}