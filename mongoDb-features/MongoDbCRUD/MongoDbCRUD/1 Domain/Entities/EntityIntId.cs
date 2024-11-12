namespace MongoDbCRUD._1_Domain.Entities;

/// <summary>
/// Key в сущности может быть Int
/// Mongo Db не генерирует  int Key при вставке новго элемента (как в Guid), нужно самомоу присваивать уникальные Key
/// </summary>
public class EntityIntId
{
    public int Id { get; }
    public string Name { get; private set; }
    
    public EntityIntId(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public void SenName(string name)
    {
        Name = name;
    }
}