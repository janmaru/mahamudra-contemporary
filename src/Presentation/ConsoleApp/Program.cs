using Mahamudra.Contemporary;
using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging;
using Primes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        private static Task<int> Sum(BaseEntity<int> number)
        {
            return Task.FromResult(PrimeFactory.PrimeNumbersLessEqualToN(number.Id).Sum());
        }

        public static void Main(string[] args)
        { 
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<P>();

            var primes = PrimeFactory.PrimeNumbersLessEqualToN(10).Select(x => new NumberOfPrimes(x)); // find all primes less or equal to 10 --> 3,5,7

            var p = new P(logger);
            p.Execute<BaseEntity<int>, int>(primes, Sum);

            // wait for stroke
            Console.ReadKey(true);
        }
    }
}
