using PrimeNumberGeneratorLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MainApp
{
    class Program
    {
        const int RUNTIME_IN_SECONDS = 60;

        // Setting the prime number limit to 70% of int.MaxValue because it gave me the best results
        // using the sieve Eratosthenes algorithm for finding primes. 
        const int PRIME_NUMBER_LIMIT = (int)(((long)int.MaxValue * 70) / 100);

        static Stopwatch stopwatch;
        static string primeOutput = string.Empty;
        static int highestPrimeNumber;

        static void Main(string[] args)
        {
            stopwatch = Stopwatch.StartNew();
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += HandleTimerElapsed;
            timer.Start();

            // Adjusted duration for running the calculation so we stop all execution within 60 seconds
            TimeSpan duration = TimeSpan.FromSeconds(RUNTIME_IN_SECONDS) - stopwatch.Elapsed;

            SieveEratosthenes se = new SieveEratosthenes();

            // using Tasks and CancellationToken to calculate prime numbers and stop by timeout
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(duration);
            Task.Run(() => se.CalculatePrimeNumbers(PRIME_NUMBER_LIMIT, cts.Token))
                                .ContinueWith((t) =>
                                {
                                    stopwatch.Stop();
                                    timer.Stop();
                                    timer.Elapsed -= HandleTimerElapsed;
                                    
                                    if (primeOutput != string.Empty)
                                    {
                                        Console.Write(primeOutput);
                                    }

                                    Console.WriteLine(string.Format("Highest prime number calculated: {0}", highestPrimeNumber));
                                });
                                    
            // Consumer task that builds the prime number display
            Task.Factory.StartNew(() =>
            {
                foreach (int value in se.PrimeNumbers.GetConsumingEnumerable(cts.Token))
                {
                    primeOutput += string.Format("{0},", value);
                    highestPrimeNumber = value;
                }
            });

            Console.ReadLine();
        }

        private static void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Elapsed in seconds: {0}s", stopwatch.Elapsed.Seconds);
            Console.WriteLine(primeOutput);
            primeOutput = string.Empty;
        }
    }
}
