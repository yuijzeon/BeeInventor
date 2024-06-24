using BenchmarkDotNet.Running;

namespace BeeInventor.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<SimpleDictionaryStage>();
        BenchmarkRunner.Run<WildCardDictionaryStage>();
    }
}