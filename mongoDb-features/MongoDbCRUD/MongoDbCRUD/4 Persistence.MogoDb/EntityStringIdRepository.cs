using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;

namespace MongoDbCRUD._4_Persistence.MogoDb;

public class EntityStringIdRepository
{
    private readonly IMongoCollection<EntityStringId> _entitys;
    private readonly ConnectionThrottlingPipeline _connectionThrottlingPipeline;
    public const string CollectionName = "EntityStringIds";

    public EntityStringIdRepository(IMongoCollection<EntityStringId> entitys, ConnectionThrottlingPipeline connectionThrottlingPipeline)
    {
        _entitys = entitys;
        _connectionThrottlingPipeline = connectionThrottlingPipeline;
    }


    public async Task<List<EntityStringId>> Get(Expression<Func<EntityStringId, bool>> predicate)
    {
        return await _entitys.Find(predicate).ToListAsync();
    }
    
    
    public async Task<EntityStringId> GetSingleAsync(Expression<Func<EntityStringId, bool>> predicate)
    {
        using var cursor = await _entitys.FindAsync(predicate);
        return await cursor.SingleAsync();
    }
    
    
    public async Task<string?> AddOrReplace(EntityStringId entity)
    {
        if (entity.Key == null)
        {
            throw new ArgumentException("entity.Key cant be null");
        }
        
        var res= await _connectionThrottlingPipeline.AddRequest(
            _entitys.ReplaceOneAsync(c =>c.Key == entity.Key, entity, new ReplaceOptions{IsUpsert = true}));
        if (res.ModifiedCount == 0)
        {
            return null;
        }
        
        return entity.Key;
    }
    
    
    public async Task<bool> Delete(string entityId)
    {
        var res = await _entitys.DeleteOneAsync(e=>e.Key == entityId);
        return res.IsAcknowledged && res.DeletedCount == 1;
    }
    
    
    public async Task<long> Delete(Expression<Func<EntityStringId, bool>> predicate)
    {
        var res = await _entitys.DeleteManyAsync(predicate);
        return res.DeletedCount;
    }
}