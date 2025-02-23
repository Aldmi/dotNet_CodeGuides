using Xunit.Abstractions;

namespace TasksSample.Tests;

public class CustomException(string message) : Exception(message);


public class ManyTasksInParallel
{
	private readonly ITestOutputHelper _output;
	private readonly Task[] _taskArray;
	
	public ManyTasksInParallel(ITestOutputHelper output)
	{
		_output = output;
		_taskArray =
		[
			//Task.Factory.StartNew работает также
			Task.Run(() => WaitAndThrow(1, 1000)), 
			Task.Run(() => WaitAndThrow(2, 2000)),
			Task.Run(() => WaitAndThrow(3, 3000))
		];
	}
	
	
	private void WaitAndThrow(int id, int waitInMs)
	{
		_output.WriteLine($"{DateTime.UtcNow}: Task {id} started");
		Thread.Sleep(waitInMs);
		throw new CustomException($"Task {id} throwing at {DateTime.UtcNow}");
	}

	
	[Fact]
	public void WaitAll_Expected_All_Exceptions()
	{
		try
		{
			Task.WaitAll(_taskArray);
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
	
	[Fact]
	public async Task WaitAll_Expected_First_Exception()
	{
		try
		{
			await Task.WhenAll(_taskArray);
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
}