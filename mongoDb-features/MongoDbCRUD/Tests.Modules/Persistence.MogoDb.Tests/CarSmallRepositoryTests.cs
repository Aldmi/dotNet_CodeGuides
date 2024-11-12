using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson.Serialization;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;
using MongoDbCRUD._4_Persistence.MogoDb;
using Tests.Modules.Fixtures;
using Xunit;

namespace Tests.Modules.Persistence.MogoDb.Tests;

[Collection("SharedDbCollection")]
public class CarSmallRepositoryTests
{
    public CarSmallRepositoryTests(DatabaseFixture fixture)
    {
        Fixture = fixture;
        CarSmallRepository = new CarSmallRepository(Fixture.CarsSmall, Fixture.ConnectionThrottlingPipeline);
        Fixture.Cleanup();
    }

    private DatabaseFixture Fixture { get; }
    private CarSmallRepository CarSmallRepository { get; }
    
    
    [Fact]
    public async Task GetAll()
    {
        var carSmalls= await CarSmallRepository.Get(_=>true);  //Будет исключение если в маппере не вызвать (SetIgnoreExtraElements) System.FormatException Element 'CreatedData' does not match any field or property of class Е
        
        carSmalls.Should().SatisfyRespectively(
            one =>
            {
                one.Name.Should().Be("Car 1");
                one.Customer.Should().NotBeNull();
                one.Customer!.Name.Should().Be("Customer 1");
            },
            two =>
            {
                two.Name.Should().Be("Car 2");
                two.Customer.Should().NotBeNull();
                two.Customer!.Name.Should().Be("Customer 2");
            },
            three =>
            {
                three.Name.Should().Be("Car 3");
                three.Customer.Should().BeNull();
            },
            four =>
            {
                four.Name.Should().Be("Car 4");
                four.Customer.Should().NotBeNull();
                four.Customer!.Name.Should().Be("Customer 4");
            }
            );
    }

    [Fact]
    public async Task GetAll_With_Filter_Sorting_Limit()
    {
        var allCars= await CarSmallRepository.Get(_=>true, car =>car.Name, 2);
        allCars.Should().SatisfyRespectively(
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
         var car = new CarSmall("Car99",  customer);

         //Act
         var addedCarGuid= await CarSmallRepository.AddOrReplace(car);
         
         //Assert
         var allCars= await CarSmallRepository.Get(_=>true);
         //addedCar.Should().NotBeNull();
         allCars.Should().HaveCount(5);
    }
    
    
    [Fact]
    public async Task AddOrReplace_Update_ChangeCustomer()
    {
        //Arrange
        var allCars= await CarSmallRepository.Get(_=>true);
        var firstCar = allCars.First();
        firstCar.ChangeCustomer(new Customer("new Customer222", "new Lenina 222"));
        
        //Act
        var updatedCar= await CarSmallRepository.AddOrReplace(firstCar);
        
        //Assert
        var allCarsExpected= await CarSmallRepository.Get(_=>true);
        var expectedFirst= allCarsExpected.First(c => c.Id == firstCar.Id);

       // addedCar.Should().BeNull();
        allCarsExpected.Should().HaveCount(4);
        expectedFirst.Customer!.Name.Should().Be(firstCar.Customer!.Name);
    }
    
    
    [Fact]
    public async Task Delete_By_Id()
    {
        var firstCar = (await CarSmallRepository.Get(_ => true)).First();

        var res=await CarSmallRepository.Delete(firstCar.Id);
        var countAfterDelete = (await CarSmallRepository.Get(_ => true)).Count;
        
        res.Should().BeTrue();
        countAfterDelete.Should().Be(3);
    }
    
    
    [Fact]
    public async Task Delete_By_Predicate()
    {
        var deletedCount= await CarSmallRepository.Delete(car =>car.Customer != null);
        var countAfterDelete = (await CarSmallRepository.Get(_ => true)).Count;
        
        deletedCount.Should().Be(3);
        countAfterDelete.Should().Be(1);
    }
}