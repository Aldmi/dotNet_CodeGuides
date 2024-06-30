namespace Domain.Entities;

public class Product
{
    public Product(Guid productId, string name, int count, decimal cost, DateTime createTime)
    {
        ProductId = productId;
        Name = name;
        Count = count;
        Cost = cost;
        CreateTime = createTime;
    }

    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public int Count { get; private set; }
    public decimal Cost { get; private set; }
    public DateTime CreateTime { get; private set; }
    public DateTime? UpdateTime { get; private set; }
}