using System.Collections.Concurrent;
using FluentAssertions;
using Xunit.Abstractions;

namespace Tests;

public class DictionaryConcurrencyTests(ITestOutputHelper output)
{
    void PrintDictionary(IDictionary<int, string> dict)
    {
        output.WriteLine("PrintDictionary ----------------------------- ");
        foreach (var pair in dict)
        {
            output.WriteLine($"[{pair.Key}]= {pair.Value}");
        }
    }

    [Fact]
    public async Task Dictionary_Concurrency_Add_RepeatKey_Faild()
    {
        //arrange
        Dictionary<int, string> dictionary = [];

        void Add(string value)
        {
            for (var i = 0; i < 10; i++)
            {
                var message = $"Added By {value} {i}";
                output.WriteLine(message);
                dictionary.Add(i, message);
                Thread.Sleep(5);
            }
        }

        //act
        Func<Task> act = async () =>
        {
            var task1 = Task.Run(() => Add("Add_1"));
            var task2 = Task.Run(() => Add("Add_2"));
            await Task.WhenAll(task1, task2);
            PrintDictionary(dictionary);
        };

        //assert
        //ключи словаря должны быть уникальными,
        //а ключ был продублирован одним из методов. Ошибка была получена, потому что обобщённый словарь по умолчанию не обеспечивает потокобезопасность.
        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    
    [Fact]
    public async Task ConcurrentDictionary_Concurrency_Add_RepeatKey_Success()
    {
        //act
        ConcurrentDictionary<int, string> dictionary = [];

        void TryAdd(string value)
        {
            for (var i = 0; i < 10; i++)
            {
                var message = $"Added By {value} {i}. Thread= {Environment.CurrentManagedThreadId}";
                output.WriteLine(message);
                dictionary.TryAdd(i, message);
                Thread.Sleep(5);
            }
        }

        Func<Task> act = async () =>
        {
            var task1 = Task.Run(() => TryAdd("1"));
            var task2 = Task.Run(() => TryAdd("2"));
            await Task.WhenAll(task1, task2);
            PrintDictionary(dictionary);
        };

        //act, assert
        await act.Should().NotThrowAsync<Exception>();
    }


    [Fact]
    public async Task ConcurrentDictionary_20Tasks_incrementBy1000()
    {
        ConcurrentDictionary<int, int> concurrentDictionary = [];

        var taskArray = new Task<int>[20];
        for (var i = 0; i < taskArray.Length; i++)
        {
            concurrentDictionary.TryAdd(i, 0);

            taskArray[i] = Task.Factory.StartNew(taskParameter =>
            {
                var key = Convert.ToInt32(taskParameter);
                for (var j = 0; j < 1000; j++)
                {
                    concurrentDictionary.TryGetValue(key, out var current);
                    concurrentDictionary.TryUpdate(key, current + 1, current);
                }

                var valueRetrieved = concurrentDictionary.TryGetValue(key, out var result);
                if (valueRetrieved)
                    return result;

                throw new Exception($"No data item available for key {taskParameter}");
            }, i);
        }

        var resultArray = await Task.WhenAll(taskArray);
        var resultSum = resultArray.Sum();

        resultSum.Should().Be(20 * 1000);
    }


    [Fact]
    public async Task Dictionary_IEnumearable_Exception_CollectionWasModified()
    {
        //act
        Dictionary<int, string> dictionary = [];

        void TryAdd(string value)
        {
            for (var i = 0; i < 100; i++)
            {
                var message = $"Added By {value} {i}";
                output.WriteLine(message);
                dictionary.TryAdd(i, message);
                Thread.Sleep(1);
            }
        }

        void read()
        {
            Thread.Sleep(10);
            foreach (var pair in dictionary)
            {
                output.WriteLine(pair.Key.ToString());
                Thread.Sleep(1);
            }
        }

        Func<Task> act = async () =>
        {
            var task1 = Task.Run(() => TryAdd("Add"));
            var task2 = Task.Run(() => read());
            await Task.WhenAll(task1, task2);
        };

        //act, assert
        await act.Should().ThrowAsync<Exception>();
    }


    [Fact]
    public async Task Dictionary_IEnumearable_Exception_CollectionWasModified_Fixed()
    {
        //act
        Dictionary<int, string> dictionary = [];

        void TryAdd(string value)
        {
            for (var i = 0; i < 100; i++)
            {
                var message = $"Added By {value} {i}";
                output.WriteLine(message);
                dictionary.TryAdd(i, message);
                Thread.Sleep(1);
            }
        }

        void read()
        {
            Thread.Sleep(10);
            //Создать копию коллекции для перечисления 
            var copyDictionary = dictionary.ToDictionary();
            foreach (var pair in copyDictionary)
            {
                output.WriteLine(pair.Key.ToString());
                Thread.Sleep(1);
            }
        }

        Func<Task> act = async () =>
        {
            var task1 = Task.Run(() => TryAdd("Add"));
            var task2 = Task.Run(() => read());
            await Task.WhenAll(task1, task2);
        };

        //act, assert
        await act.Should().NotThrowAsync<Exception>();
    }


    /// <summary>
    /// Иногда InvalidOperationException не вылетает и код добавления в словарь заканчивается без ошибок
    /// </summary>
    [Fact]
    public async Task RoleManagerDictionary_Add_RepeatKey_Faild()
    {
        Func<Task> act = async () =>
        {
            var roleManager = new RoleManagerDictionary();
            var tasks = Enumerable.Range(1, 100)
                .Select(i => roleManager.TryAssign("Admin", i))
                .ToList();
            await Task.WhenAll(tasks);
        };
        
        //act, assert
        await act.Should().ThrowAsync<Exception>(); //Exception: Operations that change non-concurrent collections must have exclusive access.
    }
    
    
    [Fact]
    public async Task RoleManagerConcurrentDictionary_Add_RepeatKey_Succsess()
    {
        Func<Task> act = async () =>
        {
            var roleManager = new RoleManagerConcurrentDictionary();
            var tasks = Enumerable.Range(1, 100)
                .Select(i => roleManager.TryAssign("Admin", i))
                .ToList();
            await Task.WhenAll(tasks);
        };
        
        //act, assert
        await act.Should().NotThrowAsync<InvalidOperationException>();
    }
}