
using BenchmarkDotNet.Running;
using ParallelExample;

Console.WriteLine("Hello, World!");
var invokeApiParallelBenchmark = new InvokeApiParallelBenchmark();
var res= await invokeApiParallelBenchmark.AsyncParallelForTask_5();
Console.WriteLine($"{res}");


BenchmarkRunner.Run<InvokeApiParallelBenchmark>();