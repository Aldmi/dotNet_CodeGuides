namespace WebApi.Domain.Entityes;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    /// <summary>
    /// Видимость товара в продаже
    /// </summary>
    public bool IsSaleEnabled { get; set; }
    
    public int StockCount { get; set; }
}