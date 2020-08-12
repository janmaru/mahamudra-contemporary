using System.Collections.Generic;

namespace Primes
{
    public class PrimeFactory
    {
        public static List<int> PrimeNumbersLessEqualToN(int n)
        {
            List<int> primes = new List<int>();
 
            int i, j, isPrime;
 
            for (i = 1; i <= n; i++)
            {
 
                if (i == 1 || i == 0)
                    continue;

                isPrime = 1;

                for (j = 2; j <= i / 2; ++j)
                {
                    if (i % j == 0)
                    {
                        isPrime = 0;
                        break;
                    }
                }
 
                if (isPrime == 1)
                    primes.Add(i);
            }
            return primes;
        }
    }
}
