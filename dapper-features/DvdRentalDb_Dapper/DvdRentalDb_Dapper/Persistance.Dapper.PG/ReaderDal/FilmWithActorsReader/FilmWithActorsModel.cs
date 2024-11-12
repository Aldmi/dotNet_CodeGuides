using DvdRentalDb_Dapper.Domain;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.ReaderDal.FilmWithActorsReader;

internal class FilmWithActorsModel
{
    public const string TableName = "film";

    public int FilmId { get; set; }
    public string Title { get; init; }
    public string Description { get; init; }
    // public int ReleaseYear { get; init; }
    // public int RentalDuration { get; init; }
    // public double RentalRate { get; init; }
    // public int Length { get; init; }
    // public double ReplacementCost { get; init; }
    
    public string Rating { get; init; }
    public string Language { get; init; }
    public string[] SpacialFeatures { get; init; }
    public DateTime LastUpdate { get; init; }
    public string ActorsNameList { get; init; }
}