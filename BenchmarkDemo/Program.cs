using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

namespace BenchmarkDemo
{
    [MemoryDiagnoser]
    public class Program
    {
        static List<int> ints = new List<int>();
        static Program()
        {
            for (int i = 0; i < 1_000_000_000; i++)
            {
                ints.Add(i);
            }
        }
        static void Main(string[] args)
        {
            
            var summary = BenchmarkRunner.Run<Program>();
            Console.WriteLine(summary);
            Console.ReadLine();

        }

        [Benchmark]
        public static void Demo1()
        {
            int count = ints.Count;
            for (int i = 0; i < count; i++)
            {
                int val = ints[i];
            }

        }
        [Benchmark]
        public static void Demo2()
        {
            foreach(var v in ints)
            {
                int val = v;
            }

        }
    }
}
