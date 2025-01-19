namespace Ef_Paging.Entities;

public class Product
{
    public Guid ProductId { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; init; }
}