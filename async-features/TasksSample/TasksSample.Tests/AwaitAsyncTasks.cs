using Xunit.Abstractions;

namespace TasksSample.Tests;

/// <summary>
/// Обрабатывать результат task по мере завершения
/// </summary>
public class AwaitAsyncTasks(ITestOutputHelper testOutputHelper)
{


	[Fact]
	public async Task WhenEach_Ok()
	{
		//arrange
		var tasks = Enumerable.Range(1, 10).Select(i=>Calculate(i)).ToList();
		
		//act
		await foreach (var task in Task.WhenEach(tasks))
		{
			var result = await task;
			testOutputHelper.WriteLine(result.ToString());
		}
	}
	
	
	/// <summary>
	/// Прекращение работы при первом исключении (незавершенные задачи продолжат выполняться)
	/// </summary>
	[Fact]
	public async Task WhenEach_Exception_Global_Handler()
	{
		//arrange
		const int orderAfterThrow = 8;
		var tasks = Enumerable.Range(1, 10).Select(i=>Calculate(i, orderAfterThrow)).ToList();
		
		//act
		try
		{
			await foreach (var task in Task.WhenEach(tasks))
			{
				var result = await task;
				testOutputHelper.WriteLine(result.ToString());
			}
		}
		catch (Exception e)
		{
			testOutputHelper.WriteLine(e.ToString());
		}
	}


	/// <summary>
	/// Вывод результата от каждой задачи
	/// </summary>
	[Fact]
	public async Task WhenEach_Exception_Local_Handler()
	{
		//arrange
		const int orderAfterThrow = 8;
		var tasks = Enumerable.Range(1, 10).Select(i => Calculate(i, orderAfterThrow)).ToList();

		//act
		await foreach (var task in Task.WhenEach(tasks))
		{
			try
			{
				var result = await task;
				testOutputHelper.WriteLine(result.ToString());
			}
			catch (Exception e)
			{
				testOutputHelper.WriteLine(e.ToString());
			}
		}
	}


	private async Task<(int order, int time)> Calculate(int order, int? orderAfterThrow = null)
	{
		var number= Random.Shared.Next(1000,5000);
		await Task.Delay(number);
		if (orderAfterThrow == order)
		{
			throw new Exception("order after throw");
		}
		return (order, number);
	}
}