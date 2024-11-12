namespace MongoDbCRUD._1_Domain.Entities;

/// <summary>
/// Центр обслуживания машины
/// </summary>
public class ServiceCenter
{
    public string Name { get; }

    public ServiceCenter(string name)
    {
        Name = name;
    }
}