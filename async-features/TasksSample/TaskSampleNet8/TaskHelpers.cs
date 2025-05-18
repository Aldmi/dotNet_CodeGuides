using Nito.AsyncEx;

namespace TaskSampleNet8;

public static class TaskHelpers
{
	/// <summary>
	/// Вернуть задачи по мере завершения
	/// При первом исключенири внутри задачи остальные задачи не будут выведены (хотя продолжат выполнение)
	/// (.net9 уже есть эта функция)
	/// </summary>
	/// <param name="tasks"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static async IAsyncEnumerable<T> WhenEach<T>(this IEnumerable<Task<T>> tasks)
	{
		foreach (var task in tasks.OrderByCompletion())
		{
			yield return await task;
		}
	}
	
	/// <summary>
	/// Вернуть задачи по мере завершения
	/// (.net9 уже есть эта функция)
	/// </summary>
	/// <param name="tasks"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static async IAsyncEnumerable<T> WhenEachExceptionIgnore<T>(this IEnumerable<Task<T>> tasks)
	{
		foreach (var task in tasks.OrderByCompletion())
		{
			T result = default;
			try
			{
				result= await task;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			yield return result;
		}
	}
}