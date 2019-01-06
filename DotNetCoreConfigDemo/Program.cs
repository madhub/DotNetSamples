using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConfigDemo
{
    /// <summary>
    /// DemoHostedService 
    /// </summary>
    public class DemoHostedService : IHostedService
    {
        public DemoHostedService(IEventBus eventBus)
        {
            Console.WriteLine("MyHostedService called ...");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StartAsync called ...");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StopAsync called ...");
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// Base implementation for all console based service
    /// </summary>
    public class DemoBackgroundService : BackgroundService
    {
        private readonly IEventBus eventBus;

        public DemoBackgroundService(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
                return Task.CompletedTask;
            stoppingToken.Register(() =>
            {
               // stp[ background activity
           });
            // start background activity
            return Task.CompletedTask;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBus
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class DummyEventBus : IEventBus
    {
        public DummyEventBus(EventBusConfiguration eventBusConfiguration)
        {
            Console.WriteLine(eventBusConfiguration);
        }
    }
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
    /// extension method to invoke starup class 
    /// </summary>
    public static class HostBuilderExtension
    {
        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder) where TStartup : IStartup
        {
            hostBuilder.ConfigureServices(
                (hostBuilderContext, serviceCollection) =>
            {
                var implementationType = typeof(TStartup);
                var statupInstance = Activator.CreateInstance(implementationType) as IStartup;
                statupInstance.ConfigureService(hostBuilderContext, serviceCollection);
                serviceCollection.AddSingleton(typeof(IStartup), statupInstance);

            });
            return hostBuilder;
        }
    }
    /// <summary>
    /// Base interface for all console based service to register specific dependencies
    /// </summary>
    public interface IStartup
    {
        void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection);
    }
    public class BackgoundServiceStartup : IStartup
    {

        public void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            Console.WriteLine("Configure Service Called. Adding BackgoundService specific service registration ..");
            // Register Event Bus 
            serviceCollection.AddTransient<IEventBus, DummyEventBus>();
            // Register configuration option
            serviceCollection.Configure<EventBusConfiguration>(hostBuilderContext.Configuration.GetSection("eventBusConfiguration"));
            // Explicitly register the settings object by delegating to the IOptions object
            // https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-asp-net-core/
            serviceCollection.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<EventBusConfiguration>>().Value);

        }
    }
   

    class Program
    {
       
        static async Task Main(string[] args)
        {

            // Generic Hosted Build for console Application
            var builder = new HostBuilder().ConfigureServices((hostBuilderContext, services) =>
           {
               Console.WriteLine("Main.ConfigureServices");
               services.AddHostedService<DemoHostedService>();
           })
           .UseStartup<BackgoundServiceStartup>()
           .ConfigureAppConfiguration(configBuilder =>
           {
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

               configBuilder.AddJsonFile("appconfig.json");
               configBuilder.AddEnvironmentVariables();
               configBuilder.AddCommandLine(args);
           });

            await builder.RunConsoleAsync();


            //Console.WriteLine(dom.GetSection("eventBusConfiguration:connection")["hostname"]);
            //// https://weblog.west-wind.com/posts/2017/Dec/12/Easy-Configuration-Binding-in-ASPNET-Core-revisited
            //var config = new EventBusConfiguration();
            //dom.Bind("eventBusConfiguration", config);     

            Console.ReadLine();


        }

    }
}
