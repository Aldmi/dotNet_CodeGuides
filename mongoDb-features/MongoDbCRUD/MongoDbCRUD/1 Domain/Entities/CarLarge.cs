

namespace MongoDbCRUD._1_Domain.Entities;

/// <summary>
/// Car после рефакторинга.
/// Добавили Mileage, CarType
/// </summary>
public class CarLarge : Car
{
    public int Mileage { get; set; }
    public CarType CarType { get; set; }
    
    
    public CarLarge(string name, DateTime createdData, Customer? customer, List<ServiceCenter>? serviceCenters, int mileage, CarType carType) 
        : base(name, createdData, customer, serviceCenters)
    {
        Mileage = mileage;
        CarType = carType;
    }


    public void ChnageCarType(CarType carType)
    {
        CarType = carType;
    }
}

public enum CarType
{
    None,
    PassengerCar,
    Track,
    Bus
}
