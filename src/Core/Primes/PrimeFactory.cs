using System;
using System.Collections.Generic;

namespace Primes
{
    public class PrimeFactory
    {
        public static IEnumerable<int> FirstNIntegers(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                yield return i;
            }
        }

        public static List<int> PrimeNumbersLessEqualToNThrowsExceptionOn5(int n)
        {
            if (n == 5)
                throw new Exception("I don't like the number five.");
            else
                return PrimeNumbersLessEqualToN(n);
        }

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
