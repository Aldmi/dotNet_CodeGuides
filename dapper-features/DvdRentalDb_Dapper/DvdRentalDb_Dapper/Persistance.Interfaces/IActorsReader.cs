using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using DvdRentalDb_Dapper.Domain;

namespace DvdRentalDb_Dapper.Persistance.Interfaces;

public interface IActorsReader
{
    Result<ReadOnlyCollection<Actor>> GetActorsByCriteria(ActorSearchCriteria?  criteria = default, CancellationToken ct= default);
}

public class ActorSearchCriteria
{
    public int? Id { get; init; }
    public string? FirstName { get; init;}
    public string? LastName { get; init; }
    public DateTime? MoreLastUpdate { get; init; }
    public DateTime? LessLastUpdate { get; init; }
}