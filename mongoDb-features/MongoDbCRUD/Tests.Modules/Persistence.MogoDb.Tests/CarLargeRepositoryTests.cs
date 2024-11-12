using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._4_Persistence.MogoDb;
using Tests.Modules.Fixtures;
using Xunit;

namespace Tests.Modules.Persistence.MogoDb.Tests;

[Collection("SharedDbCollection")]
public class CarLargeRepositoryTests : IDisposable
{
    public CarLargeRepositoryTests(DatabaseFixture fixture)
    {
        Fixture = fixture;
        CarLargeRepository = new CarLargeRepository(Fixture.CarsLarge, Fixture.ConnectionThrottlingPipeline);
        Fixture.Cleanup();
    }

    private DatabaseFixture Fixture { get; }
    private CarLargeRepository CarLargeRepository { get; }
    
    
    [Fact]
    public async Task GetAll()
    {
        //Конструктор CarLarge не подходит для маппинга (т.к. у элемента в БД меньше свойств чем в новом типа CarLarge), поэтому объект будет создан и свойства инициализтрованны через setter
        // private init; для свйоства необходим, иначе не сможем настроить объект.
        var cars= await CarLargeRepository.Get(_=>true); 
        
        cars.Should().SatisfyRespectively(
            one =>
            {
                one.Name.Should().Be("Car 1");
                one.Customer.Should().NotBeNull();
                one.Customer!.Name.Should().Be("Customer 1");
                one.ServiceCenters.Should().NotBeNull();
                one.ServiceCenters!.Should().HaveCount(3);
                one.Mileage.Should().Be(0);
                one.CarType.Should().Be(CarType.None);
            },
            two =>
            {
                two.Name.Should().Be("Car 2");
                two.Customer.Should().NotBeNull();
                two.Customer!.Name.Should().Be("Customer 2");
                two.ServiceCenters.Should().NotBeNull();
                two.ServiceCenters!.Should().HaveCount(2);
                two.Mileage.Should().Be(0);
                two.CarType.Should().Be(CarType.None);
            },
            three =>
            {
                three.Name.Should().Be("Car 3");
                three.Customer.Should().BeNull();
                three.ServiceCenters.Should().NotBeNull();
                three.ServiceCenters!.Should().HaveCount(3);
                three.Mileage.Should().Be(0);
                three.CarType.Should().Be(CarType.None);
            },
            four =>
            {
                four.Name.Should().Be("Car 4");
                four.Customer.Should().NotBeNull();
                four.Customer!.Name.Should().Be("Customer 4");
                four.ServiceCenters.Should().BeNull();
                four.Mileage.Should().Be(0);
                four.CarType.Should().Be(CarType.None);
            }
        );
    }


    [Fact]
    public async Task GetAll_With_Filter_Sorting_Limit()
    {
        var cars= await CarLargeRepository.Get(_=>true, car =>car.Name, 2);
        cars.Should().SatisfyRespectively(
            one =>
            {
                one.Name.Should().Be("Car 1");
            },
            two =>
            {
                two.Name.Should().Be("Car 2");
            }
        );
    }


    [Fact]
    public async Task AddOrReplace_Add_New()
    {
         //Arrange
         var customer = new Customer("Customer999", "Lenina 999");
         var serviceCenters = Enumerable.Range(0, 5).Select(index => new ServiceCenter($"ServiceCenter {index}")).ToList();
         var car = new CarLarge("Car99", DateTime.Now, customer, serviceCenters, 10, CarType.PassengerCar);

         //Act
         var addedCarGuid= await CarLargeRepository.AddOrReplace(car);
         
         //Assert
         var cars= await CarLargeRepository.Get(_=>true);
         //addedCar.Should().NotBeNull();
         cars.Should().HaveCount(5);
    }
    
    
    [Fact]
    public async Task AddOrReplace_Update_ChangeCustomer()
    {
        //Arrange
        var cars= await CarLargeRepository.Get(_=>true);
        var firstCar = cars.First();
        firstCar.ChangeCustomer(new Customer("new Customer222", "new Lenina 222"));
        firstCar.ChnageCarType(CarType.Bus);
        
        //Act
        var updatedCarId= await CarLargeRepository.AddOrReplace(firstCar);
        
        //Assert
        var allCarsExpected= await CarLargeRepository.Get(_=>true);
        var expectedFirst= allCarsExpected.First(c => c.Id == firstCar.Id);

        updatedCarId.Should().Be(firstCar.Id);
        allCarsExpected.Should().HaveCount(4);
        expectedFirst.Customer!.Name.Should().Be(firstCar.Customer!.Name);
    }
    
    
    [Fact]
    public async Task Delete_By_Id()
    {
        var firstCar = (await CarLargeRepository.Get(_ => true)).First();

        var res=await CarLargeRepository.Delete(firstCar.Id);
        var countAfterDelete = (await CarLargeRepository.Get(_ => true)).Count;
        
        res.Should().BeTrue();
        countAfterDelete.Should().Be(3);
    }
    
    
    [Fact]
    public async Task Delete_By_Predicate()
    {
        var deletedCount= await CarLargeRepository.Delete(car =>car.Customer != null);
        var countAfterDelete = (await CarLargeRepository.Get(_ => true)).Count;
        
        deletedCount.Should().Be(3);
        countAfterDelete.Should().Be(1);
    }
    
    
    
    public void Dispose()
    {
        Fixture.Cleanup();
    }
}