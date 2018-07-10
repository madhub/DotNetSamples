using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNetCoreConfigDemo
{
    public class Connection
    {
        public string serviceName { get; set; }
        public string hostName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        
    }

    public class ExchangeDetail
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class QueueDetail
    {
        public string name { get; set; }
        public string exchangeName { get; set; }
    }

    public class EventBusConfiguration
    {
        public string eventBusType { get; set; }
        public int RetryCount { get; set; }
        public Connection connection { get; set; }
        public List<ExchangeDetail> exchangeDetails { get; set; }
        public List<QueueDetail> queueDetails { get; set; }
    }
    /// <summary>
    /// sample implementation of custom configuration provider
    /// 
    /// </summary>
    public class MyConfig : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyConfigurationProvider();
        }
    }

    public class MyConfigurationProvider : ConfigurationProvider
    {
        Dictionary<string, string> mykeys = new Dictionary<string, string> { { "Timezone", "+1" } };
        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            if (parentPath == null)
            {
                return new List<string>(earlierKeys) { "Timezone" };
            }
            return earlierKeys;
        }

        public override void Load()
        {
            Console.WriteLine("");
        }

        public override void Set(string key, string value)
        {
            mykeys[key] = value;
        }

        public override bool TryGet(string key, out string value)
        {
            return mykeys.TryGetValue(key, out value);
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection();
            
           
            // Env is a reference to the IHostingEnvironment instance
            // that you might want to inject in the class via ctor
            var dom = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("appconfig.json")
              .Add(new MyConfig())
              .AddEnvironmentVariables()
              .AddCommandLine(args)
              .Build();
            // Test 1.  Run the sample, should see console output "localhost" as configured in appconfig.json
            // Test 2.  Set environment variable eventBusConfiguration:connection:hostname to envhost , 
            //          and run the program , should see see console output "envhost" . This overrdes the value 
            //          configured in appconfig.json
            //          Example :   set eventBusConfiguration:connection:hostname=envhost
            //                      dotnet DotNetCoreConfigDemo.dll
            // Test 3.  Add command line argument eventBusConfiguration:connection:hostname=cmdhost , 
            //          and run the program , should see see console output "cmdhost" . This overrdes the value 
            //          configured in appconfig.json & environment variable 
            //          Example: dotnet DotNetCoreConfigDemo.dll eventBusConfiguration:connection:hostname=cmdhost

            
            Console.WriteLine(dom.GetSection("eventBusConfiguration:connection")["hostname"]);
            var config = new EventBusConfiguration();
            dom.Bind("eventBusConfiguration", config);      //  <--- This
            //dom.Bind()
            //var section = serviceProvider.Configure<EventBusConfiguration>(dom.GetSection("eventBusConfiguration"));
            //serviceProvider
            ////IOptions<PagingOptions> config
            Console.ReadLine();
        }
       
    }
}
