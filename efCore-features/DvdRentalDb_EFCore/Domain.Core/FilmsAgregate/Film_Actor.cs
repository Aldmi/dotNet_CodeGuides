namespace Domain.Core.FilmsAgregate;

public class Film_Actor
{
    public Film Film { get; set; }
    public Actor Actor { get; set; }
    public DateTime LastUpdate { get; private set; }
}