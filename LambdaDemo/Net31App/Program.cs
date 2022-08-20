using BenchmarkDotNet.Running;
using System;

namespace Net31App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run(typeof(Net31App.StringIteration));
        }
    }
}
