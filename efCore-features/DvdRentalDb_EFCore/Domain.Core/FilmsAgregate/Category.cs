namespace Domain.Core.FilmsAgregate;

public class Category
{
    public int CategoryId { get; private set; }
    public string Name { get; private set; }
    public DateTime LastUpdate { get; private set; }
}