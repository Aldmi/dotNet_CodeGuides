using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;

namespace ParallelExample;

public class InvokeApiParallelBenchmark
{
    private static readonly HttpClient HttpClient = new();
    private const int TaskCount = 20;


    //Последовательное выполнение асинхронных задач.
    [Benchmark]
    public async Task<List<int>> ForEachVersion()
    {
        var list = new List<int>();
        var funcs = Enumerable.Range(0, TaskCount)
            .Select(_ => new Func<Task<int>>(() => GetWetherForecastAsync(HttpClient)))
            .ToList();

        foreach (var task in funcs)
        {
            var result = await task();
            list.Add(result);
        }

        return list;
    }


    //Паралельное выполнение синхронных задач.
    //очень не эффективно работатет процессор, ядра всегда загруженны (синхроннл дожидается результата выполнения)
    [Benchmark]
    public List<int> ParallelVersion() => ParallelVersion(-1);
 

    //Паралельное выполнение синхронных задач с ограничением паралелизма.
    [Benchmark]
    public List<int> LimitedParallelVersion() => ParallelVersion(2);

    
    [Benchmark]
    public async Task<List<int>> WhenAllVersion()
    {        
        //Создаем список холодных задач.
        var tasks = Enumerable.Range(0, TaskCount)
            .Select(_ => GetWetherForecastAsync(HttpClient));
        
        var result = await Task.WhenAll(tasks);
        return result.ToList();
    }
    
    
    //
    [Benchmark]
    public Task<List<int>> AsyncParallelVersion_1() => AsyncParallelForFuncVersion(1);
    
    
    //
    [Benchmark]
    public Task<List<int>> AsyncParallelVersion_5() => AsyncParallelForFuncVersion(5);
    
    
    //
    [Benchmark]
    public Task<List<int>> AsyncParallelVersion_25() => AsyncParallelForFuncVersion(25);
    
    
    //
    [Benchmark]
    public Task<List<int>> AsyncParallelVersion_50() => AsyncParallelForFuncVersion(50);
    
    
    //
    [Benchmark]
    public Task<List<int>> AsyncParallelForTask_5() => AsyncParallelForTaskVersion(5);
    
    
    private List<int> ParallelVersion(int maxDegreeOfParallelizm)
    {
        var list = new List<int>();
        var funcs = Enumerable.Range(0, TaskCount)
            .Select(_ =>
                new Func<int>(() => GetWetherForecastAsync(HttpClient).GetAwaiter().GetResult())) //синхронное выполнение (Parallel не поддерживает асинхронность)
            .ToList();

        Parallel.For(
            0,
            funcs.Count,
            new ParallelOptions {MaxDegreeOfParallelism = maxDegreeOfParallelizm},
            i =>
            {
                var result = funcs[i]();
                list.Add(result);
            });

        return list;
    }

    
    
    /// <summary>
    /// Разбиение данных на батчи и паралельный запуск обработки каждого батча.
    /// внутри батча выполняется асинхронная обработка.
    /// НАИБОЛЕЕ ЭФФЕКТИТВНО.
    /// </summary>
    /// <param name="batches"></param>
    /// <returns></returns>
    private async Task<List<int>> AsyncParallelForFuncVersion(int batches)
    {        
        //Создаем список асинхронных вызываемых методов.
        var list = new List<int>();
        var funcs = Enumerable.Range(0, TaskCount)
            .Select(_ =>
                new Func<Task<int>>(() => GetWetherForecastAsync(HttpClient))) //
            .ToList();

        await ParallelExtensions.ParallelForEachAsync(
            funcs,
            batches,
            async func => list.Add(await func()));
        
        return list;
    }
    
    
    private async Task<List<int>> AsyncParallelForTaskVersion(int batches)
    {        
        //Создаем список холодных задач.
        var list = new List<int>();
        var tasks = Enumerable.Range(0, TaskCount)
            .Select(_ => GetWetherForecastAsync(HttpClient));
        
        await ParallelExtensions.ParallelForEachAsync(
            tasks,
            batches,
            async task => list.Add(await task));
        
        return list;
    }
    
    
    private static async Task<int> GetWetherForecastAsync(HttpClient httpClient)
    {
        var response = await httpClient.GetFromJsonAsync<List<WeatherForecast>>("http://localhost:5098/weatherforecast");
        return response?.Count ?? 0;
    }
    
    
    // private static async Task<int> GetWetherForecastAsync(HttpClient httpClient)
    // {
    //     await Task.Delay(10);
    //     return 10;
    // }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
