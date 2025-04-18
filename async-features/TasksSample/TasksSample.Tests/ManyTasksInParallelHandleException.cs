using TasksSample.Tests.TaskExtensions;
using Xunit.Abstractions;

namespace TasksSample.Tests;

public class CustomException(string message) : Exception(message);


public class ManyTasksInParallelHandleException
{
	private readonly ITestOutputHelper _output;
	private readonly IEnumerable<Task> _tasks;
	
	public ManyTasksInParallelHandleException(ITestOutputHelper output)
	{
		_output = output;
		_tasks = Enumerable.Range(1, 3).Select(i => Task.Run(() => WaitAndThrow(i, i * 1000)));
	}
	
	
	private void WaitAndThrow(int id, int waitInMs)
	{
		_output.WriteLine($"{DateTime.UtcNow}: Task {id} started");
		Thread.Sleep(waitInMs);
		throw new CustomException($"Task {id} throwing at {DateTime.UtcNow}");
	}

	
	/// <summary>
	/// Синхронное ожидание всех задач, позволяет получить все исключения под AggregateException
	/// </summary>
	[Fact]
	public void WaitAll_Expected_All_Exceptions()
	{
		try
		{
			Task.WaitAll(_tasks);
			_output.WriteLine("This isn't going to happen");
		}
		catch (AggregateException ex)
		{
			foreach (var inner in ex.InnerExceptions)
			{
				_output.WriteLine($"Caught AggregateException in Main at {DateTime.UtcNow}: " + inner.Message);
			}
		}
		catch (Exception ex)
		{
			_output.WriteLine($"Caught Exception in Main at {DateTime.UtcNow}: " + ex.Message);
		}
		_output.WriteLine("Done.");
	}
	
	/// <summary>
	/// Асинхронное ожидание всех задач, получаем только первое исключение.
	/// </summary>
	[Fact]
	public async Task WhenAll_Expected_First_Exception()
	{
		try
		{
			await Task.WhenAll(_tasks);
			_output.WriteLine("This isn't going to happen");
		}
		catch (AggregateException ex)
		{
			foreach (var inner in ex.InnerExceptions)
			{
				_output.WriteLine($"Caught AggregateException in Main at {DateTime.UtcNow}: " + inner.Message);
			}
		}
		catch (Exception ex)
		{
			_output.WriteLine($"Caught Exception in Main at {DateTime.UtcNow}: " + ex.Message);
		}
		_output.WriteLine("Done.");
	}
	
	/// <summary>
	///  TaskExt.WhenAll - Позволяет дождаться все потоки и вывести исключения из всех потоков
	/// </summary>
	[Fact]
	public async Task TaskExt_WhenAll_Expected_All_Exception()
	{
		try
		{
			await TaskExt.WhenAll(_tasks.ToArray());
			_output.WriteLine("This isn't going to happen");
		}
		catch (AggregateException ex)
		{
			foreach (var inner in ex.InnerExceptions)
			{
				_output.WriteLine($"Caught AggregateException in Main at {DateTime.UtcNow}: " + inner.Message);
			}
		}
		catch (Exception ex)
		{
			_output.WriteLine($"Caught Exception in Main at {DateTime.UtcNow}: " + ex.Message);
		}
		_output.WriteLine("Done.");
	}
	
	/// <summary>
	///  TaskExt.WhenAll - Перехватит Exception из t3Exс и t4Exc и вернет их под AggregateException
	///  Task.WhenAll - Перехватит Exception только t3Exс под AggregateException
	/// WhenAll - ждет завершение всех задач, даже если некоторые уже завершились Exception
	/// </summary>
	[Fact]
	public async Task TaskExt_WhenAll_Expected_All_Result()
	{
		try
		{
			var t1 = Task.Run(async () =>
			{
				await Task.Delay(500);
				return 1;
			});
			var t2 = Task.Run(async () =>
			{
				await Task.Delay(3000);
				return 2;
			});
			var t3Exс = Task.Run<int>(async () =>
			{
				await Task.Delay(1000);
				throw new CustomException($"Task {3} throwing at {DateTime.UtcNow}");
			});
			var t4Exc = Task.Run<int>(async () =>
			{
				await Task.Delay(1200);
				throw new CustomException($"Task {4} throwing at {DateTime.UtcNow}");
			});
			
			var res= await TaskExt.WhenAll(t1, t2, t3Exс, t4Exc);
			foreach (var r in res)
			{
				_output.WriteLine($"{r}");
			}
		}
		catch (AggregateException ex)
		{
			foreach (var inner in ex.InnerExceptions)
			{
				_output.WriteLine($"Caught AggregateException in Main at {DateTime.UtcNow}: " + inner.Message);
			}
		}
		catch (Exception ex)
		{
			_output.WriteLine($"Caught Exception in Main at {DateTime.UtcNow}: " + ex.Message);
		}
		_output.WriteLine("Done.");
	}
}
