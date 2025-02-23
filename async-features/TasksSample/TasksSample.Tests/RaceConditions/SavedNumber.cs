namespace TasksSample.Tests.RaceConditions;

public class SavedNumber
{
	private const string Path = $@"D:\C#\Samples\dotNet_CodeGuides\async-features\TasksSample\TasksSample.Tests\RaceConditions\number.txt";//D:\C#\Samples\dotNet_CodeGuides\async-features\TasksSample\TasksSample.Tests\RaceConditions\number.txt


	/// <summary>
	/// Инициализация файла
	/// </summary>
	public SavedNumber()
	{
		File.WriteAllText(Path, "0");
	}


	public async Task IncrementAsync()
	{
		var text = await File.ReadAllTextAsync(Path);
		var number = int.Parse(text);
		number++;
		await File.WriteAllTextAsync(Path, number.ToString());
	}
}