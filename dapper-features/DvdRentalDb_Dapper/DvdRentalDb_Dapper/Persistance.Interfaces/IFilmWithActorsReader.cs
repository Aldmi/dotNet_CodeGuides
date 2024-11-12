using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using DvdRentalDb_Dapper.Domain;

namespace DvdRentalDb_Dapper.Persistance.Interfaces;

public interface IFilmWithActorsReader
{
    Result<ReadOnlyCollection<Film>> GetActorsByCriteria(FilmSearchCriteria?  criteria = default, CancellationToken ct= default);
}

public class FilmSearchCriteria
{
    public int? Id { get; init; }
    public string? Title { get; init;}
    public int? ReleaseYear { get; init; }
    
    public int Offset { get; init; }
    public int Limit { get; init; }
}