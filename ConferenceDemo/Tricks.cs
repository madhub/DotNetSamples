using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceDemo
{
    public class Tricks
    {
        public static void PrintInts()
        {
            var items = new[] { 1, 2, 3, 4, 5 };
            Console.WriteLine(string.Join(",", items.Select(i => i.ToString())));
        }
    }
}
