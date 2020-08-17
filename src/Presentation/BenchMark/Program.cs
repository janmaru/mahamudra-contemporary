using BenchmarkDotNet.Running;
using System;

namespace Mahamudra.Contemporary.BenchMark
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ParallelFactoryBenchmarks>();
            BenchmarkRunner.Run<ParallelAsyncFactoryBenchmarks>();
            Console.ReadKey(true);
        }
    }
}
