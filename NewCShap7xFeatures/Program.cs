using System;
using System.Collections.Generic;
using System.IO;
using static CommonUtils.CommonExtensions;

namespace NewCShap7xFeatures
{
    public class Person
    {
        // Auto-Properties Initializers
        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        // Expression Bodied Functions and Properties
        public override string ToString() => FirstName + LastName;
        // Additionally, C# 6 allows us to filter the exceptions that should be 
        // caught not only based on the exception's type
        public void SomeDemo()
        {
            try
            {
                var file = File.OpenRead(null);
            }
            catch (Exception exp) when ( exp as ArgumentNullException == null)
            {
                Console.WriteLine("argument null");                
            }
            catch (Exception exp) when (exp as IOException != null)
            {

            }
            catch (Exception exp) when (exp.InnerException as ArgumentNullException != null)
            {

            }


        }
        

    }
    class Program
    {

        // Auto-Properties Initializers
        public ConsoleColor Background { get; } = ConsoleColor.Green;

        static void Main1(string[] args)
        {

            //string s = (DateTime.Now.Day > 20 ? "Month end" : throw new Exception("Too early!"));

            // Null-conditional Operator
            string nullvalue = null;
            string message = nullvalue?.ToUpper() ?? "HELLO World";
            Console.WriteLine(message);

            // Dictionary Initializers
            var themesDictionary = new Dictionary<string, ConsoleColor>
            {
                ["blue"]=ConsoleColor.Blue,
                ["green"]=ConsoleColor.Green
            };
        }
        public static void Main(string[] args)
        {
            try
            {
                PrintName(null);
            }
            catch (Exception exp)
            {

                Console.WriteLine(exp);
            }

            Console.ReadKey();
            
        }

        public static void PrintName(string name)
        {
            name.ThrowIfArgumentIsNull(nameof(name));
        }
    }
}
