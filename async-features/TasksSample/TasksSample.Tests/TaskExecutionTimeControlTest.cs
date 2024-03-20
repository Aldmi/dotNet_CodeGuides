using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit.Abstractions;

namespace TasksSample.Tests;

/// <summary>
///Контроль Времени выполнения задачи.
/// WaitAsync - возвращает задачу которая успешно завершится если завершится вложенная задача или вылетит TimeoutException.
/// </summary>
public class TaskExecutionTimeControlTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TaskExecutionTimeControlTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Long_Running_Expected_TimeoutException()
    {
        //arrange
        var task1 = Task.Delay(TimeSpan.FromSeconds(1));
        Func<Task> act = () => task1.WaitAsync(TimeSpan.FromSeconds(0.8));
        
        //act
        //assert
        await act.Should().ThrowAsync<TimeoutException>();
    }
    
    
    [Fact]
    public async Task Normal_Running_Expected_Ok()
    {
        //arrange
        var task1 = Task.Delay(TimeSpan.FromSeconds(1));
        var task2 = task1.WaitAsync(TimeSpan.FromSeconds(2.0));
        
        //act
        await task2;
        
        //assert
        task2.IsCompletedSuccessfully.Should().BeTrue();
    }
    
    
    
    [Fact]
    public async Task Task_not_Canceled_After_TimeOut_invoke()
    {
        //arrange
        var task1 = async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.2));
                _testOutputHelper.WriteLine($"in task3 {i}");
            }
            return true;
        };
        
        try
        {
            var res= await task1().WaitAsync(TimeSpan.FromSeconds(1));
        }
        catch (TimeoutException e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }

        //_testOutputHelper.WriteLine($"in main code {task1.Status}");
        await Task.Delay(TimeSpan.FromSeconds(4));
       // _testOutputHelper.WriteLine($"in main code END {task1.Status}");
    }
    
    
    
    [Fact]
    public async Task Task_Canceled_After_TimeOut_invoke_With_CancelationTokenSource()
    {
        //arrange
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        {
            try
            {
                var res = await Task.Run<bool>(async () =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await Task.Delay(TimeSpan.FromSeconds(0.2), cts.Token);
                        _testOutputHelper.WriteLine($"in task {i}");
                    }
                    return true;
                }, cts.Token);
            }
            catch (TaskCanceledException e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
        }
        
        _testOutputHelper.WriteLine($"in main code");
        await Task.Delay(TimeSpan.FromSeconds(10));
    }

}