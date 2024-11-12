namespace DvdRentalDb_Dapper.Persistance.Dapper.PG.ReaderDal.ActorsReader;

public class ActorModel
{
    public const string TableName = "actor";
    public int ActorId { get; set; }
    public string FirstName { get; init;}
    public string LastName { get; init; }
    public DateTime LastUpdate { get; init; }
}


