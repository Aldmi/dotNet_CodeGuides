using FluentAssertions;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace TasksSample.Tests.RaceConditions;


public class RaceConditionsTests (ITestOutputHelper output)
{
	private const int NumberOfTasks= 1000;
	
	/// <summary>
	/// Exception  возникает при одновременном редактировании файла.
	/// </summary>
	[Fact]
	public async Task Increment_Throw_Exception()
	{
		var number = new SavedNumber();
		var tsc= new TaskCompletionSource();
		var work = async () =>
		{
			await tsc.Task;
			await number.IncrementAsync();
		};
		
		var tasks = new List<Task>();
		for (int i = 0; i < NumberOfTasks; i++)
		{
			var t = work();
			tasks.Add(t);
		}
		
		tsc.SetResult(); //После SetResult начнется одновременное выполнение задач.
		var action = async () =>
		{
			await Task.WhenAll(tasks);
		};

		action.Should().ThrowAsync<InvalidOperationException>();
	
		output.WriteLine($"{number}");
	}
	
	
	
	/// <summary>
	/// Приходится внутри lock использовать синхронную версию, что может вызвать голод потоков.
	/// </summary>
	[Fact]
	public async Task Increment_lock()
	{
		var number = new SavedNumber();
		var tsc= new TaskCompletionSource();
		object locker = new object();
		var work = async () =>
		{
			await tsc.Task;
			lock (locker)
			{
				number.IncrementAsync().GetAwaiter().GetResult();//Придется использовать синхронную версию 
			}
		};
		
		var tasks = new List<Task>();
		for (int i = 0; i < NumberOfTasks; i++)
		{
			var t = work();
			tasks.Add(t);
		}
		
		tsc.SetResult(); //После SetResult начнется одновременное выполнение задач.
		await Task.WhenAll(tasks);
		output.WriteLine($"{number}");
	}
	
	
	/// <summary>
	/// Новый mutex вырожденная версия семафора с 1 потоком
	/// Внутри критической секции можно использовать async
	/// Но mutex также использует синхронное ожидание что вызовет голод потоков
	/// </summary>
	[Fact]
	public async Task Increment_mutex()
	{
		var number = new SavedNumber();
		var tsc= new TaskCompletionSource();
		var mutex = new Mutex();
		var work = async () =>
		{
			await tsc.Task;
			mutex.WaitOne();
			await number.IncrementAsync();
			mutex.ReleaseMutex();
		};
		
		var tasks = new List<Task>();
		for (int i = 0; i < NumberOfTasks; i++)
		{
			var t = work();
			tasks.Add(t);
		}
		
		tsc.SetResult(); //После SetResult начнется одновременное выполнение задач.
		await Task.WhenAll(tasks);
		output.WriteLine($"{number}");
	}
	
	
	/// <summary>
	/// SemaphoreSlim позволяет синхронизировать потоки и ждет вход в критическую секцию асинхронно
	/// </summary>
	[Fact]
	public async Task Increment_semaphore()
	{
		var number = new SavedNumber();
		var tsc= new TaskCompletionSource();
		var semaphore = new SemaphoreSlim(1);
		var work = async () =>
		{
			await tsc.Task;
			await semaphore.WaitAsync();//Ждем асинхронно
			await number.IncrementAsync();
			semaphore.Release();
		};
		
		var tasks = new List<Task>();
		for (int i = 0; i < NumberOfTasks; i++)
		{
			var t = work();
			tasks.Add(t);
		}
		
		tsc.SetResult(); //После SetResult начнется одновременное выполнение задач.
		await Task.WhenAll(tasks);
		output.WriteLine($"{number}");
	}
	
	
	/// <summary>
	/// Если несколько приложений имеют общий ресурс.
	/// Распределенная блокировка для общекго ресурса через Redis
	/// используем nuget StackExchange.Redis
	/// </summary>
	[Fact]
	public async Task Increment_External_Lock_With_Redis()
	{
		var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
		var db = connectionMultiplexer.GetDatabase();
		
		var number = new SavedNumber();
		var tsc= new TaskCompletionSource();
		var work = async () =>
		{
			await tsc.Task;
			
			//получаем распределенную блокировку
			while (!await db.LockTakeAsync("number", "value", TimeSpan.FromSeconds(10))) //Проверяем что ресурс не занят другим приложением раз в 5 мс (expire - гарантированно снимет блокировку, через это время.)
			{
				await Task.Delay(5);
			}

			try
			{
				await number.IncrementAsync();
			}
			catch (Exception e)
			{
			}
			finally
			{
				db.LockRelease("number", "value"); //снимаем блокировку. (удаляется ключ в redis)
			}
		};
		
		var tasks = new List<Task>();
		for (int i = 0; i < NumberOfTasks; i++)
		{
			var t = work();
			tasks.Add(t);
		}
		
		tsc.SetResult(); //После SetResult начнется одновременное выполнение задач.
		await Task.WhenAll(tasks);
		output.WriteLine($"{number}");
	}
	
}
