using MongoDbCRUD._1_Domain.Base;

namespace MongoDbCRUD._1_Domain.Entities;

public class Car : AgregationRoot
{
    public string Name { get; private set;}
    public DateTime CreatedData { get; private init;}
    public Customer? Customer { get; private set; }
    public List<ServiceCenter>? ServiceCenters { get; private init;}


    
    public Car(string name, DateTime createdData, Customer? customer, List<ServiceCenter>? serviceCenters)
    {
        Name = name;
        CreatedData = createdData;
        Customer = customer;
        ServiceCenters = serviceCenters;
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