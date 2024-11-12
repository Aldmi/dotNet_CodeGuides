using System.Data;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.DbConnection;


/// <summary>
/// Подключение к БД берет через context EF
/// </summary>
public class DbConnectFromEfContext : IDbConnect
{

    public DbConnectFromEfContext() //передавать EfDbContext
    {
        
    }
    
    
    public IDbConnection GetConnection()
    {
        //retirn _context.Database.GetDbConnection()
        throw new NotImplementedException();
    }
}