using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using System;

namespace Mahamudra.Contemporary.BenchMark
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance);
            config.AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance));
            BenchmarkRunner.Run<ParallelFactoryBenchmarks>(config);
            BenchmarkRunner.Run<ParallelAsyncFactoryBenchmarks>(config); 
            Console.ReadKey(true);
        }
    }
}
