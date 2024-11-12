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
public class EntityIntIdRepositoryTests
{
        public EntityIntIdRepositoryTests(DatabaseFixture fixture)
        {
            Fixture = fixture;
            EntityIntIdRepository = new EntityIntIdRepository(Fixture.EntityIntIds, Fixture.ConnectionThrottlingPipeline);
            Fixture.Cleanup();
        }
    
        private DatabaseFixture Fixture { get; }
        private EntityIntIdRepository EntityIntIdRepository { get; }


        [Fact]
        public async Task GetAll()
        {
            var all = await EntityIntIdRepository.Get(_ => true);
            all.Should().SatisfyRespectively(
                one =>
                {
                    one.Id.Should().Be(1);
                    one.Name.Should().Be("Entity_1");
                },
                two =>
                {
                    two.Id.Should().Be(2);
                    two.Name.Should().Be("Entity_2");
                },
                three =>
                {
                    three.Id.Should().Be(3);
                    three.Name.Should().Be("Entity_3");
                },
                four =>
                {
                    four.Id.Should().Be(4);
                    four.Name.Should().Be("Entity_4");
                }
            );
        }


        [Fact]
        public async Task GetSingleAsync_Ok()
        {
            var single = await EntityIntIdRepository.GetSingleAsync(e=>e.Id == 3);

            single.Name.Should().Be("Entity_3");
        }
        
        
        [Fact]
        public async Task GetSingleAsync_Exception_not_one_element()
        {
            Func<Task<EntityIntId>> act = () => EntityIntIdRepository.GetSingleAsync(e=>e.Id > 1);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Sequence contains more than one element");;
        }


        [Fact]
        public async Task AddOrReplace_Add_New()
        {
            var entity = new EntityIntId (10,  "Entyty3");
            var id = await EntityIntIdRepository.AddOrReplace(entity);
            
            
            var all = await EntityIntIdRepository.Get(i => i.Id == entity.Id);
            id.Should().BeNull();
            all.Count.Should().Be(1);
            all.First().Should().BeEquivalentTo(entity);
        }
        
        
        [Fact]
        public async Task AddOrReplace_Update_ChangeName()
        {
            //Arrange
            var first= (await EntityIntIdRepository.Get(_=>true)).First();
            first.SenName("New Name");
        
            //Act
            var updatedId= await EntityIntIdRepository.AddOrReplace(first);
        
            //Assert
            var allExpected= await EntityIntIdRepository.Get(_=>true);
            var expectedFirst= allExpected.First(c => c.Id == first.Id);

            updatedId.Should().Be(first.Id);
            allExpected.Should().HaveCount(4);
            expectedFirst.Should().BeEquivalentTo(first);
        }
        
        
        [Fact]
        public async Task Delete_By_Id()
        {
            var firstCar = (await EntityIntIdRepository.Get(_ => true)).First();
    
            var res=await EntityIntIdRepository.Delete(firstCar.Id);
            var countAfterDelete = (await EntityIntIdRepository.Get(_ => true)).Count;
            
            res.Should().BeTrue();
            countAfterDelete.Should().Be(3);
        }
        
        
        [Fact]
        public async Task Delete_By_Predicate()
        {
            var deletedCount= await EntityIntIdRepository.Delete(e =>e.Id > 2);
            var countAfterDelete = (await EntityIntIdRepository.Get(_ => true)).Count;
        
            deletedCount.Should().Be(2);
            countAfterDelete.Should().Be(2);
        }
}