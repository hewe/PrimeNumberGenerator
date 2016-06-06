using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace PrimeNumberGeneratorLib
{
    /// <summary>
    /// Prime number generator that implements Sieve of Eratosthenes algorithm.
    /// </summary>
    public class SieveEratosthenes
    {
        /// <summary>
        /// Used as producer and consumer collection on the prime numbers
        /// </summary>
        BlockingCollection<int> _primeNums = new BlockingCollection<int>();
        public BlockingCollection<int> PrimeNumbers
        {
            get
            {
                return _primeNums;
            }
        }

        /// <summary>
        /// Generates all prime numbers under a given limit.
        /// </summary>
        /// <param name="limit">The upper limit for prime number generator. </param>
        /// <param name="ct">The cancellation token in order to cancel the prime number calculation task. </param>
        /// <returns>An <see cref="IEnumerable{int}"/> of prime numbers</returns>
        public IEnumerable<int> CalculatePrimeNumbers(int limit, CancellationToken ct = new CancellationToken())
        {
            int sieveLimit = (int)Math.Sqrt(limit);
            BitArray primeBitArr = new BitArray(limit + 1, true);

            primeBitArr[0] = false;
            primeBitArr[1] = false;

            for(int i = 2; i <= sieveLimit; i++)
            {
                if (primeBitArr[i])
                {
                    _primeNums.Add(i);

                    for (long j = i * i; j <= limit; j+= i)
                    {
                        primeBitArr[(int)j] = false;
                    }
                }
                ct.ThrowIfCancellationRequested();
            }
            
            for (int i = sieveLimit + 1; i < primeBitArr.Count; i++)
            {
                if (primeBitArr[i])
                {
                    _primeNums.Add(i);
                }
                ct.ThrowIfCancellationRequested();
            }
            return _primeNums;
        }
    }
}
