using EntityFrameworkCore.Projectables;
using WebApi.Domain.Entityes;

namespace WebApi.Domain.Specifications;

/// <summary>
/// Методы расширения задают правила фильтрации для сущностей.
/// аттрибуи Projectable конвертирует лямбду в Expression, который использует EF
/// </summary>
public static class ProductExt
{
    [Projectable]
    public static bool IsAvailable(this Product p) =>
        p.IsSaleEnabled && p.StockCount > 0;
}