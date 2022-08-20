using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program { 


        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(20);
            Console.WriteLine($"Sending Http Request http://localhost:5000/weatherforecast/something/abc wating for result");
            try
            {
                var watch = Stopwatch.StartNew();
                var result = client.GetAsync("http://localhost:5000/weatherforecast/something/abc", HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
                watch.Stop();
                Console.WriteLine($"Response Received in {watch.Elapsed.TotalSeconds} seconds, {watch.Elapsed.TotalMinutes} mins, ");
            }
            catch (Exception exp)
            {

                Console.WriteLine($"Exception {exp}");
            }
            
            

        }
        static void Main11(string[] args)
        {
            var numbers = Enumerable.Range(1, 128).ToArray();
            
            Console.WriteLine(BinarySearch(numbers, 20));

            int BinarySearch(int[] numbers, int item)
            {
                int low = 0;
                int high = numbers.Length - 1;
                while (low <= high)
                {
                    int mid = (high + low) / 2;
                    if (numbers[mid] == item)
                    {
                        return mid;
                    }
                    else if (numbers[mid] < item)
                    {
                        low = mid + 1;
                    }
                    else
                    {
                        high = mid - 1;
                    }
                }

                return 1;
            }

        }



        public static async IAsyncEnumerable<Stream> GetStream()
        {
            //await Task.Delay(1000);
            yield return Stream.Null;

        }

        public static Stream GetStream1()
        {
            return Stream.Null;
        }
    }
}
