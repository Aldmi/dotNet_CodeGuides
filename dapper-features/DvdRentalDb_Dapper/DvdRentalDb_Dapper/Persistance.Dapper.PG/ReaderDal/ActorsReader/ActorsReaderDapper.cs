using System.Collections.ObjectModel;
using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using DvdRentalDb_Dapper.Domain;
using DvdRentalDb_Dapper.Persistance.Dapper.PG.DalModel;
using DvdRentalDb_Dapper.Persistance.Dapper.PG.DbConnection;
using DvdRentalDb_Dapper.Persistance.Dapper.PG.ReaderDal.ActorsReader;
using DvdRentalDb_Dapper.Persistance.Interfaces;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.ReadDalService;

public class ActorsReaderDapper : IActorsReader
{
    private readonly IDbConnect _dbConnect;
    public ActorsReaderDapper()
    {
        _dbConnect = new DbConnectFactory();
    }
    
    
    public Result<ReadOnlyCollection<Actor>> GetActorsByCriteria(ActorSearchCriteria? criteria = default, CancellationToken ct = default)
    {
        var res= Result.Try(() =>
        {
            using var db = _dbConnect.GetConnection();
            db.Open();
            
            var listTable = db.Query<ActorModel>(BuildSearchQuery(criteria), criteria).ToList();

            //TODO: через automapper
            var list = listTable.Select(item => new Actor
            {
                ActorId = item.ActorId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                LastUpdate = item.LastUpdate
            }).ToList(); 
            return list.AsReadOnly();
        });
       return res;
    }
    
    
    private string BuildSearchQuery(ActorSearchCriteria? criteria)
    {
        var query = new StringBuilder();
        query.AppendLine($@"SELECT
                            actor_id as {nameof(ActorModel.ActorId)},
                            first_name as {nameof(ActorModel.FirstName)},
                            last_name as {nameof(ActorModel.LastName)},
                            last_update as {nameof(ActorModel.LastUpdate)}
                            FROM {ActorModel.TableName}");
            
        query.AppendLine("WHERE 1=1 ");
        
        if (criteria?.FirstName != null)
        {
            query.AppendLine($"AND first_name = @{nameof(ActorSearchCriteria.FirstName)}");
        }
        if (criteria?.LastName != null)
        {
            query.AppendLine($"AND last_name = @{nameof(ActorSearchCriteria.LastName)}");
        }
        if (criteria?.MoreLastUpdate != null)
        {
            query.AppendLine($"AND last_update > @{nameof(ActorSearchCriteria.MoreLastUpdate)}");
        }

        query.Append(';');
            
        var sumQuery=query.ToString();
        return sumQuery;
    }
}