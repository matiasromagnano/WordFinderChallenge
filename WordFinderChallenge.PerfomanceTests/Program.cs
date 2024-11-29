using BenchmarkDotNet.Running;
using WordFinderChallenge.PerfomanceTests;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<WordFinderControllerPerformanceTests>();
        Console.ReadLine();
    }
}
