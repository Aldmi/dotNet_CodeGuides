using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;
using MongoDbCRUD._4_Persistence.MogoDb;
using Tests.Modules.Fixtures;
using Xunit;

namespace Tests.Modules.Persistence.MogoDb.Tests;

[Collection("SharedDbCollection")]
public class CarRepositoryTests
{
    public CarRepositoryTests(DatabaseFixture fixture)
    {
        Fixture = fixture;
        CarRepository = new CarRepository(Fixture.Cars, Fixture.ConnectionThrottlingPipeline);
        Fixture.Cleanup();
    }

    private DatabaseFixture Fixture { get; }
    private ICarRepository CarRepository { get; }
    
    
    [Fact]
    public async Task GetAll()
    {
        var allCars= await CarRepository.Get(_=>true);

        allCars.Should().SatisfyRespectively(
            one =>
            {
                one.Name.Should().Be("Car 1");
                one.Customer.Should().NotBeNull();
                one.Customer!.Name.Should().Be("Customer 1");
                one.ServiceCenters.Should().NotBeNull();
                one.ServiceCenters!.Should().HaveCount(3);
            },
            two =>
            {
                two.Name.Should().Be("Car 2");
                two.Customer.Should().NotBeNull();
                two.Customer!.Name.Should().Be("Customer 2");
                two.ServiceCenters.Should().NotBeNull();
                two.ServiceCenters!.Should().HaveCount(2);
            },
            three =>
            {
                three.Name.Should().Be("Car 3");
                three.Customer.Should().BeNull();
                three.ServiceCenters.Should().NotBeNull();
                three.ServiceCenters!.Should().HaveCount(3);
            },
            four =>
            {
                four.Name.Should().Be("Car 4");
                four.Customer.Should().NotBeNull();
                four.Customer!.Name.Should().Be("Customer 4");
                four.ServiceCenters.Should().BeNull();
            }
            );
    }

    [Fact]
    public async Task GetAll_With_Filter_Sorting_Limit()
    {
        var allCars= await CarRepository.Get(_=>true, car =>car.CreatedData, 2);
        allCars.Should().SatisfyRespectively(
            one =>
            {
                one.Name.Should().Be("Car 3");
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
         var car = new Car("Car99", DateTime.Now, customer, serviceCenters);

         //Act
         var addedCarGuid= await CarRepository.AddOrReplace(car);
         
         //Assert
         var allCars= await CarRepository.Get(_=>true);
         addedCarGuid.Should().NotBeNull();
         allCars.Should().HaveCount(5);
    }
    
    
    [Fact]
    public async Task AddOrReplace_Update_ChangeCustomer()
    {
        //Arrange
        var allCars= await CarRepository.Get(_=>true);
        var firstCar = allCars.First();
        firstCar.ChangeCustomer(new Customer("new Customer222", "new Lenina 222"));
        
        //Act
        var updatedCarId= await CarRepository.AddOrReplace(firstCar);
        
        //Assert
        var allCarsExpected= await CarRepository.Get(_=>true);
        var expectedFirst= allCarsExpected.First(c => c.Id == firstCar.Id);

        updatedCarId.Should().Be(firstCar.Id);
        allCarsExpected.Should().HaveCount(4);
        expectedFirst.Customer!.Name.Should().Be(firstCar.Customer!.Name);
    }
    
    
    [Fact]
    public async Task Delete_By_Id()
    {
        var firstCar = (await CarRepository.Get(_ => true)).First();

        var res=await CarRepository.Delete(firstCar.Id);
        var countAfterDelete = (await CarRepository.Get(_ => true)).Count;
        
        res.Should().BeTrue();
        countAfterDelete.Should().Be(3);
    }
    
    
    [Fact]
    public async Task Delete_By_Predicate()
    {
        var deletedCount= await CarRepository.Delete(car =>car.Customer != null);
        var countAfterDelete = (await CarRepository.Get(_ => true)).Count;
        
        deletedCount.Should().Be(3);
        countAfterDelete.Should().Be(1);
    }
    
    [Fact]
    public async Task Delete_By_Predicate_All()
    {
        var deletedCount= await CarRepository.Delete(car =>true);
        var countAfterDelete = (await CarRepository.Get(_ => true)).Count;
        
        deletedCount.Should().Be(4);
        countAfterDelete.Should().Be(0);
    }
}