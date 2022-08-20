using BenchmarkDotNet;
using BenchmarkDotNet.Running;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string text = System.IO.File.ReadAllText(@"C:\dev\Processes.csv");
//string [] words = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//Console.WriteLine(words.Length);
//var results = words.Take(20);
//foreach (var item in results)
//{
//    Console.WriteLine(item);
//}
var summary = BenchmarkRunner.Run(typeof(Net6.StringIteration));
