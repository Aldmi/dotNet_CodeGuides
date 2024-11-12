using MongoDB.Bson.Serialization.Attributes;
using MongoDbCRUD._1_Domain.Base;

namespace MongoDbCRUD._1_Domain.Entities;

/// <summary>
/// Car после рефакторинга.
/// Убрали CreatedData, ServiceCenters
/// </summary>

//[BsonIgnoreExtraElements]
public class CarSmall : AgregationRoot
{
    public string Name { get; private set;}
    public Customer? Customer { get; private set; }

    
    public CarSmall(string name, Customer? customer)
    {
        Name = name;
        Customer = customer;
    }


    public bool ChangeCustomer(Customer customer )
    {
        Customer = customer;
        return true;
    }
    
    
    public bool ChangeName(string name)
    {
        Name = name;
        return true;
    }
}