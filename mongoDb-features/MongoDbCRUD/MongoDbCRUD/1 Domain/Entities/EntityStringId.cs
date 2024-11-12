namespace MongoDbCRUD._1_Domain.Entities;


public class EntityStringId
{
    //public string Key { get; }
    public string Key { get; }
    public string Name { get; private set; }
    
    public EntityStringId(string key, string name)
    {
        Key = key;
        Name = name;
    }
    
    public void SenName(string name)
    {
        Name = name;
    }
}