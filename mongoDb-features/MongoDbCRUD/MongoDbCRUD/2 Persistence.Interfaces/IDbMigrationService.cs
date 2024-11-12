namespace MongoDbCRUD._2_Persistence.Interfaces;


public class MigrationDbResult
{
     public string CurrentVersion { get; init; }
     public List<string> InterimSteps { get; init; }
     public string ServerAdress { get; init; }
     public string DatabaseName { get; init; }
     public bool Success { get; init; }

     public override string ToString()
     {
          return $"[{CurrentVersion}]; '{string.Join(' ', InterimSteps)}'; {ServerAdress};  {DatabaseName}";
     }
}

public interface IDbMigrationService
{
     MigrationDbResult? RunMigrate();
}

