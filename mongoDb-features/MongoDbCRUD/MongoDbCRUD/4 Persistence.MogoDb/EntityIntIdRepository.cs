using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;

namespace MongoDbCRUD._4_Persistence.MogoDb;

public class EntityIntIdRepository
{
    private readonly IMongoCollection<EntityIntId> _entitys;
    private readonly ConnectionThrottlingPipeline _connectionThrottlingPipeline;
    public const string CollectionName = "EntityIntIds";

    public EntityIntIdRepository(IMongoCollection<EntityIntId> entitys, ConnectionThrottlingPipeline connectionThrottlingPipeline)
    {
        _entitys = entitys;
        _connectionThrottlingPipeline = connectionThrottlingPipeline;
    }


    public async Task<List<EntityIntId>> Get(Expression<Func<EntityIntId, bool>> predicate)
    {
        return await _entitys.Find(predicate).ToListAsync();
    }
    
    
    public async Task<EntityIntId> GetSingleAsync(Expression<Func<EntityIntId, bool>> predicate)
    {
        using var cursor = await _entitys.FindAsync(predicate);
        return await cursor.SingleAsync();
    }
    
    
    public async Task<int?> AddOrReplace(EntityIntId entity)
    {
        if (entity.Id == 0)
        {
            throw new ArgumentException("entity.Key cant be null");
        }
        
        var res= await _connectionThrottlingPipeline.AddRequest(
            _entitys.ReplaceOneAsync(c =>c.Id == entity.Id, entity, new ReplaceOptions{IsUpsert = true}));
        if (res.ModifiedCount == 0)
        {
            return null;
        }
        
        return entity.Id;
    }
    
    
    public async Task<bool> Delete(int entityId)
    {
        var res = await _entitys.DeleteOneAsync(e=>e.Id == entityId);
        return res.IsAcknowledged && res.DeletedCount == 1;
    }
    
    
    public async Task<long> Delete(Expression<Func<EntityIntId, bool>> predicate)
    {
        var res = await _entitys.DeleteManyAsync(predicate);
        return res.DeletedCount;
    }
    
    
}