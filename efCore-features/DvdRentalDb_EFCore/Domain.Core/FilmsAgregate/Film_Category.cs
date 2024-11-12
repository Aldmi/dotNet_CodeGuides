namespace Domain.Core.FilmsAgregate;

public class Film_Category
{
    public Film Film { get; set; }
    public Category Category { get; set; }
    public DateTime LastUpdate { get; private set; }
}