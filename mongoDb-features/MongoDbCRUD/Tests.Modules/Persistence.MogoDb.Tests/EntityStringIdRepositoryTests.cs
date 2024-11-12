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
public class EntityStringIdRepositoryTests
{
        public EntityStringIdRepositoryTests(DatabaseFixture fixture)
        {
            Fixture = fixture;
            EntityStringIdRepository = new EntityStringIdRepository(Fixture.EntityStringIds, Fixture.ConnectionThrottlingPipeline);
            Fixture.Cleanup();
        }
    
        private DatabaseFixture Fixture { get; }
        private EntityStringIdRepository EntityStringIdRepository { get; }


        [Fact]
        public async Task GetAll()
        {
            var all = await EntityStringIdRepository.Get(_ => true);
            // all.Should().SatisfyRespectively(
            //     one =>
            //     {
            //         one.Key.Should().Be(1);
            //         one.Name.Should().Be("Entity_1");
            //     },
            //     two =>
            //     {
            //         two.Key.Should().Be(2);
            //         two.Name.Should().Be("Entity_2");
            //     },
            //     three =>
            //     {
            //         three.Key.Should().Be(3);
            //         three.Name.Should().Be("Entity_3");
            //     },
            //     four =>
            //     {
            //         four.Key.Should().Be(4);
            //         four.Name.Should().Be("Entity_4");
            //     }
            // );
        }


        [Fact]
        public async Task GetSingleAsync_Ok()
        {
            var single = await EntityStringIdRepository.GetSingleAsync(e=>e.Key == "3");

            single.Name.Should().Be("Entity_3");
        }
        
        
        [Fact]
        public async Task GetSingleAsync_Exception_not_one_element()
        {
            Func<Task<EntityStringId>> act = () => EntityStringIdRepository.GetSingleAsync(e=>e.Key != "1");

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Sequence contains more than one element");;
        }


        [Fact]
        public async Task AddOrReplace_Add_New()
        {
            var entity = new EntityStringId ("10",  "Entyty3");
            var id = await EntityStringIdRepository.AddOrReplace(entity);
            
            
            var all = await EntityStringIdRepository.Get(i => i.Key == entity.Key);
            id.Should().BeNull();
            all.Count.Should().Be(1);
            all.First().Should().BeEquivalentTo(entity);
        }
        
        
        [Fact]
        public async Task AddOrReplace_Update_ChangeName()
        {
            //Arrange
            var first= (await EntityStringIdRepository.Get(_=>true)).First();
            first.SenName("New Name");
        
            //Act
            var updatedId= await EntityStringIdRepository.AddOrReplace(first);
        
            //Assert
            var allExpected= await EntityStringIdRepository.Get(_=>true);
            var expectedFirst= allExpected.First(c => c.Key == first.Key);

            updatedId.Should().Be(first.Key);
            allExpected.Should().HaveCount(4);
            expectedFirst.Should().BeEquivalentTo(first);
        }
        
        
        [Fact]
        public async Task Delete_By_Id()
        {
            var firstCar = (await EntityStringIdRepository.Get(_ => true)).First();
    
            var res=await EntityStringIdRepository.Delete(firstCar.Key);
            var countAfterDelete = (await EntityStringIdRepository.Get(_ => true)).Count;
            
            res.Should().BeTrue();
            countAfterDelete.Should().Be(3);
        }
        
        
        [Fact]
        public async Task Delete_By_Predicate()
        {
            var deletedCount= await EntityStringIdRepository.Delete(e =>e.Key != "2");
            var countAfterDelete = (await EntityStringIdRepository.Get(_ => true)).Count;
        
            deletedCount.Should().Be(2);
            countAfterDelete.Should().Be(2);
        }
}