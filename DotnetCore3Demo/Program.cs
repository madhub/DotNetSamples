using System;
using System.IO;

namespace DotnetCore3Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = null;

            string value=null;
            Greet(value,dir);
            Console.WriteLine("Hello World!");
        }

        public static void Greet(String greeting, DirectoryInfo dir)
        {
            Console.WriteLine(greeting);
        }
    }
}
