using BenchmarkDotNet.Attributes;

namespace ParallelExample;

public class BasicParallelBenchmarks
{
    [Benchmark]
    public int[] NormalForeach()
    {
        var array = new int[1_000_000];
        for (int i = 0; i < 1_000_000; i++)
        {
            array[i] = i;
        }
        return array;
    }
    
    [Benchmark]
    public int[] ParallelForeach()
    {
        var array = new int[1_000_000];
        
        Parallel.For(0, 1_000_000, i =>
        {
            array[i] = i;
        });
        return array;
    }
    
}