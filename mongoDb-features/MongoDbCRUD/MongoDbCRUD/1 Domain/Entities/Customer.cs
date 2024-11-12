namespace MongoDbCRUD._1_Domain.Entities;

/// <summary>
/// Поставщик машины
/// </summary>
public class Customer
{
    public string Name { get; }
    public string Address { get; }


    public Customer(string name, string address)
    {
        Name = name;
        Address = address;
    }
}