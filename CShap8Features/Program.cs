using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;



namespace CShap8Features
{
    // C# dyanamic
   static void ExpandoObjectDemo()
   {
            dynamic customer = new ExpandoObject();
            customer.Id = 42;
            customer.sb = new StringBuilder("a string builder");

            //if we want to dynamically add properties to this expando object,
            //we can convert it to a dictionary first and then call Add method.

            var c = (IDictionary<string, object>)customer;
            c.Add("stringPropertyName", "propertyValue");

            foreach (var item in c)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
    }
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
