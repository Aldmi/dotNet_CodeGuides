using TaskSampleNet8;

Console.WriteLine("Hello, World!");

const int orderAfterThrow = 3;
var tasks = Enumerable.Range(1, 10).Select(i=>Calculate(i, orderAfterThrow)).ToList();

try
{
	await foreach (var taskResult in tasks.WhenEach())
	{
		Console.WriteLine($"Task {taskResult}");
	}
}
catch (Exception e)
{
	Console.WriteLine(e);
}




async Task<(int order, int time)> Calculate(int order, int? orderAfterThrow = null)
{
	var number= Random.Shared.Next(1000,5000);
	await Task.Delay(number);
	if (orderAfterThrow == order)
	{
		throw new Exception("order after throw");
	}
	return (order, number);
}