using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace Mahamudra.Contemporary.BenchMark
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var app = CreateHostBuilder(args).Build();

            // init benchmark
            var config = ManualConfig.Create(DefaultConfig.Instance);
            config.AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance));
            BenchmarkRunner.Run<ParallelFactoryBenchmarks>(config);
            BenchmarkRunner.Run<ParallelAsyncFactoryBenchmarks>(config);
            // end benchmark 

            IServiceProvider serviceProvider = app.Services; 
            var pollyTestService = serviceProvider.GetRequiredService<PollyTestService>();

            // test polly retry policy
            pollyTestService.ExecuteParallelResultWillFail();

            app.Run(); 
            Console.ReadKey(true);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: true);
                  config.AddJsonFile("appsettings.Development.json", optional: true);
                  config.AddEnvironmentVariables();

                  if (args != null)
                  {
                      config.AddCommandLine(args);
                  }
              }) 
              .UseContentRoot(Directory.GetCurrentDirectory()) 
              .ConfigureServices((hostContext, services) =>
              {
                  var configuration = hostContext.Configuration;
                  var env = hostContext.HostingEnvironment;
                  services.AddSingleton<IHostEnvironment>(env);
                  services.AddLogging();

                  services.AddHttpClient<IHttpErrorService, HttpErrorService>()
                    .AddPolicyHandler(GetRetryPolicy()) 
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

                  services.AddSingleton<IHttpService, HttpService>(); 
                  services.AddSingleton<PollyTestService>();
              });

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5));
        }
    } 
}
