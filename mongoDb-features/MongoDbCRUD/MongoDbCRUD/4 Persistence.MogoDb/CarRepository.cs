using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;

namespace MongoDbCRUD._4_Persistence.MogoDb;

public class CarRepository : ICarRepository
{
    private readonly IMongoCollection<Car> _cars;
    private readonly ConnectionThrottlingPipeline _connectionThrottlingPipeline;
    public const string CollectionName = "Cars";

    public CarRepository(IMongoCollection<Car> cars, ConnectionThrottlingPipeline connectionThrottlingPipeline)
    {
        _cars = cars;
        _connectionThrottlingPipeline = connectionThrottlingPipeline;
    }


    public async Task<List<Car>> Get(Expression<Func<Car, bool>> predicate)
    {
        return await _cars.Find(predicate).ToListAsync();
    }
    
    
    public async Task<List<Car>> Get(
        Expression<Func<Car, bool>> predicate,
        Expression<Func<Car, object>>? sortBy = null,
        int? limit = null)
    {
        var findFluent = _cars.Find(predicate);
        if (sortBy != null)
        {
            findFluent = findFluent.SortBy(sortBy);
        }
        if (limit != null)
        {
            findFluent = findFluent.Limit(limit);
        }
        return await findFluent.ToListAsync();
    }


    public async Task<Car?> Get(Guid carId)
    {
       return await _cars.Find(c => c.Id == carId).FirstOrDefaultAsync();
    }
    
    
    public async Task<Guid?> AddOrReplace(Car car)
    {
        //Add (Key Set automatically)
        if (car.Id == Guid.Empty)
        {
            //await _cars.InsertOneAsync(car);
            await _connectionThrottlingPipeline.AddRequest(_cars.InsertOneAsync(car));
        }
        //Replace
        else
        {
            var res= await _connectionThrottlingPipeline.AddRequest(
                _cars.ReplaceOneAsync(c =>c.Id == car.Id, car, new ReplaceOptions{IsUpsert = true})
            );
            if (res.ModifiedCount == 0)
            {
                return null;
            }
        }
        return car.Id;
    }

    
    public async Task<bool> Delete(Guid carId)
    {
        var res = await _cars.DeleteOneAsync(c=>c.Id == carId);
        return res.IsAcknowledged && res.DeletedCount == 1;
    }
    
    
    public async Task<long> Delete(Expression<Func<Car, bool>> predicate)
    {
        var res = await _cars.DeleteManyAsync(predicate);
        return res.DeletedCount;
    }
}