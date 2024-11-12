using System.Collections.ObjectModel;
using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using DvdRentalDb_Dapper.Domain;
using DvdRentalDb_Dapper.Persistance.Dapper.PG.DbConnection;
using DvdRentalDb_Dapper.Persistance.Interfaces;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.ReaderDal.FilmWithActorsReader;

public class FilmWithActorsReaderDapper : IFilmWithActorsReader
{
    private readonly IDbConnect _dbConnect;
    public FilmWithActorsReaderDapper()
    {
        _dbConnect = new DbConnectFactory();
    }


    public Result<ReadOnlyCollection<Film>> GetActorsByCriteria(FilmSearchCriteria? criteria = default, CancellationToken ct = default)
    {
        var res= Result.Try(() =>
        {
            using var db = _dbConnect.GetConnection();
            db.Open();
            
            var listTable = db.Query<FilmWithActorsModel>(BuildSearchQuery(criteria), criteria).ToList();

            //TODO: через automapper
            var list = listTable.Select(item => new Film
            {
                 FilmId = item.FilmId,
                 Description= item.Description,
                 LastUpdate = item.LastUpdate
            }).ToList(); 
            return list.AsReadOnly();
        });
        return res;
    }
    
    private string BuildSearchQuery(FilmSearchCriteria? criteria)
    {
        var query = new StringBuilder();
        query.AppendLine($@"SELECT
                            fa.film_id as {nameof(FilmWithActorsModel.FilmId)},
                            MIN(f.title) as {nameof(FilmWithActorsModel.Title)},
                            MIN(f.description) as {nameof(FilmWithActorsModel.Description)},
                            MIN(f.rating) as {nameof(FilmWithActorsModel.Rating)},
                            MIN(f.special_features) as {nameof(FilmWithActorsModel.SpacialFeatures)},
                            MIN(f.last_update) as {nameof(FilmWithActorsModel.LastUpdate)},
                            MIN(l.name) as {nameof(FilmWithActorsModel.Language)},
                            string_agg(a.first_name, ',') as {nameof(FilmWithActorsModel.ActorsNameList)}
            FROM film as f
            JOIN film_actor fa on f.film_id = fa.film_id
            JOIN actor a on fa.actor_id = a.actor_id
            JOIN language l on f.language_id = l.language_id");
            
        query.AppendLine("WHERE 1=1 ");
        
        // if (criteria?.FirstName != null)
        // {
        //     query.AppendLine($"AND first_name = @{nameof(ActorSearchCriteria.FirstName)}");
        // }
        // if (criteria?.LastName != null)
        // {
        //     query.AppendLine($"AND last_name = @{nameof(ActorSearchCriteria.LastName)}");
        // }
        // if (criteria?.MoreLastUpdate != null)
        // {
        //     query.AppendLine($"AND last_update > @{nameof(ActorSearchCriteria.MoreLastUpdate)}");
        // }

        query.AppendLine($@"GROUP BY fa.film_id
                            ORDER BY {nameof(FilmWithActorsModel.Title)}");
        
        
        if (criteria?.Offset != null)
        {
            query.AppendLine($"OFFSET @{nameof(FilmSearchCriteria.Offset)}");
        }
        if (criteria?.Limit != null)
        {
            query.AppendLine($"LIMIT @{nameof(FilmSearchCriteria.Limit)}");
        }
        
        query.Append(';');
        var sumQuery=query.ToString();
        return sumQuery;
    }
}