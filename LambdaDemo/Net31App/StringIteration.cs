using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net31App
{
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(BenchmarkDotNet.Mathematics.NumeralSystem.Arabic)]
    public class StringIteration
    {
        static readonly string [] words;
        static StringIteration()
        {
            Console.WriteLine($"Static constuctor called");
            string text = System.IO.File.ReadAllText(@"C:\dev\Processes.csv");
            words = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Word Count {words.Length}");

        }
        [Benchmark]
        public void UsingForLoop()
        {
            int len = words.Length;
            for( int i =0; i < len; i++ )
            {
                var x = words[i];
            }
        }
        [Benchmark]
        public void UsingForEachLoop()
        {
            foreach( var item in words)
            {
                var x = item;
            }
        }
    }
}
