using System;
using System.Collections.Generic;

using System.Threading.Tasks;



namespace CShap8Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Demo().GetAwaiter().GetResult();
            
        }
        public static async Task Demo()
        {
            await foreach (var x in GetSensorData1())
            {
                Console.WriteLine($"{x}");
            }

        }

        public static async IAsyncEnumerable<int> GetSensorData1()
        {
            var r = new Random();
            while (true)
            {
                await Task.Delay(r.Next(300));
                yield return r.Next();
            }
        }
    }
}
