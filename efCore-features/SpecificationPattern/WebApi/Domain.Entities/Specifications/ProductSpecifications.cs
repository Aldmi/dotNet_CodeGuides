using EntityFrameworkCore.Projectables;
using WebApi.Domain.Entities.Models;

namespace WebApi.Domain.Entities.Specifications;

/// <summary>
/// Методы расширения задают правила фильтрации для сущностей.
/// аттрибуи Projectable конвертирует лямбду в Expression, который использует EF
/// </summary>
public static class ProductSpecifications
{
    [Projectable]
    public static bool IsAvailable(this Product p) =>
        p.IsSaleEnabled && p.StockCount > 0;
    
    
    [Projectable]
    public static bool NameStartWith(this Product p, string startStr) =>
        p.Name.StartsWith(startStr);
}
