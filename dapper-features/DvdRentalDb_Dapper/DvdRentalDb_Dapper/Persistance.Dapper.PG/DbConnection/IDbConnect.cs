using System.Data;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.DbConnection;

public interface IDbConnect
{
    IDbConnection GetConnection();
}