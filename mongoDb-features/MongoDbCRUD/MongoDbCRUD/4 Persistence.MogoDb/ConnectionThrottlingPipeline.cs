using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;

namespace MongoDbCRUD._4_Persistence.MogoDb;

/// <summary>
/// Не дает разрастаться пуллу подключений к БД.
/// limit - зададет кол-во потоков, равное половину всего пула потоков, которое одновременно имеют доступ к ресурусу (операции с БД)
/// Необходимо для устранения MongoDB.Driver.MongoWaitQueueFullException
/// Это исключения возникает при паралельном доступе к Коллекции MongoDb.
/// </summary>
public class ConnectionThrottlingPipeline //: IConnectionThrottlingPipeline
{
    private readonly Semaphore _openConnectionSemaphore;

    public ConnectionThrottlingPipeline(int maxConnectionPoolSize)
    {
        //Only grabbing half the available connections to hedge against collisions.
        //If you send every operation through here
        //you should be able to use the entire connection pool.
        int limit = (int)(maxConnectionPoolSize * 0.5);
        _openConnectionSemaphore = new Semaphore(limit,limit);
    }

    public async Task<T> AddRequest<T>( Task<T> task )
    {
        _openConnectionSemaphore.WaitOne();
        try
        {
            var result = await task;
            return result;
        }
        finally
        {
            _openConnectionSemaphore.Release();
        }
    }
    
    public async Task AddRequest(Task task )
    {
        _openConnectionSemaphore.WaitOne();
        try
        {
             await task;
        }
        finally
        {
            _openConnectionSemaphore.Release();
        }
    }
}