using System;
using System.Collections.Generic;
using MongoDbCRUD._1_Domain.Entities;

namespace Tests.Modules.SeedDatas;

public static class CarsFactory
{
    public static List<Car> SeedList { get; } = new()
    {
        new (
            "Car 1",
            DateTime.Now.AddDays(-1),
            new Customer("Customer 1", "address 1"),
            new List<ServiceCenter> {
                new("ServiceCenter 1"),
                new("ServiceCenter 2"),
                new("ServiceCenter 3"),
            }
        ),
        new (
            "Car 2",
            DateTime.Now.AddDays(-2),
            new Customer("Customer 2", "address 2"),
            new List<ServiceCenter> {
                new("ServiceCenter 10"),
                new("ServiceCenter 20")
            }
        ),
        new (
            "Car 3",
            DateTime.Now.AddDays(-10),
           null,
            new List<ServiceCenter> {
                new("ServiceCenter 100"),
                new("ServiceCenter 200"),
                new("ServiceCenter 300"),
            }
        ),
        new (
            "Car 4",
            DateTime.Now,
            new Customer("Customer 4", "address 4"),
           null
        )
    };
}