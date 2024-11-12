namespace Domain.Core.FilmsAgregate;

public class Actor
{
    public int ActorId { get; init; }
    public string FirstName { get; init;}
    public string LastName { get; init; }
    public DateTime LastUpdate { get; init; }

    public List<Film> Films { get; init; } = new();
    public List<Film_Actor> FilmActors { get;  set; } = new();
}