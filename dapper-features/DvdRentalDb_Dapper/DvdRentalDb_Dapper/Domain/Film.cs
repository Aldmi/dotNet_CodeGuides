using System.ComponentModel;
using System.Runtime.Serialization;

namespace DvdRentalDb_Dapper.Domain;

public class Film
{
    public int FilmId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int ReleaseYear { get; init; }
    public int RentalDuration { get; init; }
    public double RentalRate { get; init; }
    public int Length { get; init; }
    public double ReplacementCost { get; init; }
    public MpaaRating Rating { get; init; }
    
    /// <summary>
    /// Читаем язык из БД и в строковом виде сохраняем
    /// </summary>
    public string Language { get; init; }        
    public string[] SpacialFeatures { get; init; }

    /// <summary>
    /// Список актеров, который хрнаится в связанной таблице  Film-> FilmActor <-Actor
    /// </summary>
    public List<Actor> Actors { get; init; }
    
    public DateTime LastUpdate { get; init; }
}

public enum MpaaRating
{
    Pg,
    Pg13,
    R,
    Nc17
}

