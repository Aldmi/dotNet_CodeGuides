using System.Collections;

namespace TasksSample.Tests.TaskExtensions;

public static class TaskExt
{
	public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
	{
		var allTasks = Task.WhenAll(tasks);

		try
		{
			return await allTasks;
		}
		catch (Exception e)
		{
			//ignore
		}
		
		throw allTasks.Exception ?? new Exception("This cant possibly happen");
	}
	
	
	public static async Task WhenAll(params Task[] tasks)
	{
		var allTasks = Task.WhenAll(tasks);

		try
		{
			await Task.WhenAll(tasks);;
		}
		catch (Exception e)
		{
			//ignore
		}
		
		throw allTasks.Exception ?? new Exception("This cant possibly happen");
	}
}