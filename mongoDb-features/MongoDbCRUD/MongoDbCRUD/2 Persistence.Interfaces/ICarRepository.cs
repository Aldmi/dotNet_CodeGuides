using System.Linq.Expressions;
using MongoDbCRUD._1_Domain.Entities;

namespace MongoDbCRUD._2_Persistence.Interfaces;

public interface ICarRepository
{
    Task<List<Car>> Get(Expression<Func<Car, bool>> predicate);
    Task<List<Car>> Get(
        Expression<Func<Car, bool>> predicate,
        Expression<Func<Car, object>>? sortBy = null,
        int? limit = null);
    Task<Car?> Get(Guid carId);

    /// <summary>
    /// Добавить новую машину или переписать имеющуюся.
    /// </summary>
    /// <param name="car">машина</param>
    /// <returns>Guid добавелнной машины или null если </returns>
    Task<Guid?> AddOrReplace(Car car);

    Task<long> Delete(Expression<Func<Car, bool>> predicate);
    Task<bool> Delete(Guid carId);
}