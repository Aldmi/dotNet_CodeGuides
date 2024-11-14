using System.Reflection;

namespace AsyncEnumerable;

public class GetCustomerService
{
    public async Task HandleCustomers_Once()
    {
        var customers = await HttpClientFake.GetFromJsonAsync_Once("/api/customers");
        foreach (var customer in customers)
        {
            Console.WriteLine(customer);
        }
    }
    
    
    /// <summary>
    /// Последовательная асинхронная обработка последоавтельности.
    /// </summary>
    public async Task HandleCustomers_ByOne()
    {
        var customersEnumerator =  HttpClientFake.GetFromJsonAsync_ByOne("/api/customers");
        var tokenEnumerator = customersEnumerator
            .WhereAwait(async customer => await HttpClientFake.CheckCustomer(customer))
            .SelectAwait(async customer => await HttpClientFake.CreateTokenByCustomer(customer));
        
        await foreach (var token in tokenEnumerator)
        {
            Console.WriteLine(token);
        }
    }

    /// <summary>
    /// Паралельная обработка последоавтельности.
    /// </summary>
    public async Task HandleCustomers_Parallel()
    {
        var customersEnumerator =  HttpClientFake.GetFromJsonAsync_ByOne("/api/customers");
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 3 };
        await Parallel.ForEachAsync(customersEnumerator, parallelOptions,
            async (customer, ct) =>
            {
                var token=await HttpClientFake.CreateTokenByCustomer(customer);
                //полученный элемент можно куда то отправить или сохранить в ConcurrentCollection
            });
    }
}


/// <summary>
/// 
/// </summary>
public static class HttpClientFake
{
    /// <summary>
    /// Получить всех Customer за раз с "допустимой задержкой".
    /// </summary>
    public static async Task<IReadOnlyCollection<Customer>> GetFromJsonAsync_Once(string apiCustomers)
    {
        await Task.Delay(500);
        return Enumerable.Range(1, 10).Select(r => new Customer
        {
            Id = r,
            Name = $"Name {r}"
        }).ToList();
    }


    /// <summary>
    /// Для тестов или для "адаптера" можно IEnumerable<T> превратить в IAsyncEnumerable<T>
    /// </summary>
    public static IAsyncEnumerable<Customer> Convert_2_AsyncEnumerable(string apiCustomers)
    {
        var coolection = GetFromJsonAsync_Once(apiCustomers).GetAwaiter().GetResult();
        return coolection.ToAsyncEnumerable();
    }


    /// <summary>
    /// элементы поступают по одному с большой задержкой.
    /// И их можно асинхронно обрабатывать по мере поступления
    /// </summary>
    public static async IAsyncEnumerable<Customer> GetFromJsonAsync_ByOne(string apiCustomers)
    {
        for (int i = 0; i < 20; i++)
        {
            await Task.Delay(50);
            yield return new Customer
            {
                Id = i,
                Name = $"Name {i}"
            };
            Console.WriteLine("GetFromJsonAsync_ByOne ------ ");
        }
    }


    public static async Task<bool> CheckCustomer(Customer customer)
    {
        await Task.Delay(100);
        Console.WriteLine($"CheckCustomers {customer} ................");
        return customer is {Id: > 4 and < 12, Name: not null };
    }
    
    public static async Task<string> CreateTokenByCustomer(Customer customer)
    {
        await Task.Delay(1000);
        Console.WriteLine($"CreateTokenByCustomer {customer} !!!!!!!!!!!!!!!!!");
        return $"{customer.Id.GetHashCode()} {  customer.Name.GetHashCode()}";
    }
}