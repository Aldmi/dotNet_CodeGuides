using NpgsqlTypes;

namespace Domain.Core.FilmsAgregate;


public enum MpaaRating
{
  G,
  Pg,
  Pg13,
  R,
  Nc17
}

public class Film
{
    public int FilmId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int ReleaseYear { get; private set;  }
    public int RentalDuration { get; private set; }
    public double RentalRate { get; private set; }
    public int Lenght { get; private set; }
    public double ReplacementCost { get; private set; }
    public MpaaRating MpaaRating { get; private set; }
    public DateTime LastUpdate { get; private set; }
    public string[] SpecialFeatures { get; private set; }
    public NpgsqlTsVector Fulltext { get; private set; } //sql тип tsvector мапается на NpgsqlTsVector.

    
    //RELATION
    public Language Language { get; set; }
    
    public List<Actor> Actors { get; private set; }
    public List<Film_Actor> FilmActors { get;  private set; } = new(); //Связзующая таблица M2M
    
    
    public List<Category> Categories { get; private set; }
    public List<Film_Category> FilmCategories { get;  private set; } = new();
}