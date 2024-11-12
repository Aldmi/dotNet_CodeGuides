using System.Data;
using Npgsql;

namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.DbConnection;

public class DbConnectFactory : IDbConnect
{
    private readonly string _connectionString;
    
    
    //Передаем IConfiguration и вытаскиваем строку подключения
    public DbConnectFactory()
    {
         _connectionString = "User ID = postgres; Password = dmitr; Server = localhost; Port = 5432; Integrated Security = true; Pooling = true; Database = dvdrental";
    }
    
    
    public IDbConnection GetConnection()=>
        new NpgsqlConnection(_connectionString);
}