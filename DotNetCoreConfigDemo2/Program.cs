using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConfigDemo2
{
    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();

        private EmptyDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
    public class PollingToken : IChangeToken
    {
        Timer timer ;
        private bool changed = false;
        private CancellationTokenSource _cancellationTokenSource;
        public PollingToken()
        {
            timer = new Timer((arg) =>
           {
               Console.WriteLine("Timer Fired");
               _cancellationTokenSource.Cancel();
               changed = true;
           }, null, 4000, 5000);
            
        }
        public bool HasChanged
        {
            get
            {
                if ( changed )
                {
                    Console.WriteLine("Token Changed");

                    changed = false;
                    return true;
                }else
                {
                    Console.WriteLine("Token Not Changed");
                    //changed = false;
                    return false;
                }
                
            }
        }

        public bool ActiveChangeCallbacks => false;

        

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return _cancellationTokenSource.Token.Register(callback,state);
        }
    }
    public class MyConfigurationSource : IConfigurationSource
    {
        public int DelayBetweenChangeInMilliseconds { get; set; }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyConfigurationProvider(this);
        }
    }
    //https://www.natmarchand.fr/consul-configuration-aspnet-core/
    public class MyConfigurationProvider : ConfigurationProvider
    {

        private readonly Task _changeListenerTask;

        private  void ListenForChange()
        {
            while( true )
            {
                Thread.Sleep(4000);
                LoadNewData();
                Console.WriteLine("Invoking OnReload");
                //OnReload();
                
            }
        }

        public MyConfigurationProvider(MyConfigurationSource myConfigurationSource)
        {
            MyConfigurationSource = myConfigurationSource;

            _changeListenerTask = new Task(ListenForChange);

        }
        public override void Load()
        {
            Console.WriteLine("MyConfigurationProvider::load is called");
            LoadNewData();

            if (_changeListenerTask.Status == TaskStatus.Created)
            {
                _changeListenerTask.Start();
            }
        }

        private void LoadNewData()
        {
            this.Data.Clear();
            this.Data.Add("dicomSettings:aet", "ae1");
            this.Data.Add("dicomSettings:host", "blah");
            this.Data.Add("dicomSettings:port", DateTime.Now.ToString());
        }


        public MyConfigurationSource MyConfigurationSource { get; }
    }
    /// <summary>
    /// DemoHostedService 
    /// </summary>
    public class DemoHostedService : IHostedService
    {
        private readonly IOptionsMonitor<MessagesOptions> messagesOptions;
        private readonly IOptionsMonitor<DicomSettings> dicomSettings;
        private bool stop = false;

        public DemoHostedService(IOptionsMonitor<MessagesOptions> messagesOptions,
            IOptionsMonitor<DicomSettings> dicomSettings)
        {
            
            Console.WriteLine("MyHostedService called ...");
            this.messagesOptions = messagesOptions;
            this.dicomSettings = dicomSettings;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StartAsync called ...");
            cancellationToken.Register(() =>
           {
               stop = true;
           });
            Task.Factory.StartNew(() =>
           {
               while( !stop)
               {
                   Thread.Sleep(5000);
                   //Console.WriteLine(messagesOptions.CurrentValue);
                   Console.WriteLine(dicomSettings.CurrentValue); 
               }
           });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StopAsync called ...");
            return Task.CompletedTask;
        }
    }
    public class MessagesOptions
    {
        public string AlertMessage { get; set; }
        public string RegularMessage { get; set; }
        public bool ShouldShowAlert { get; set; }

        public override string ToString()
        {
            return $"{AlertMessage},{RegularMessage},{ShouldShowAlert}";
        }
    }
    public class SomeConfig
    {
        public string Key0 { get; set; }
        public string Key1 { get; set; }
        
    }
    public class DicomSettings
    {
        public string Aet { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public override string ToString()
        {
            return $"{Aet},{Host},{Port}";
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
                services.Configure<MessagesOptions>(hostBuilderContext.Configuration.GetSection("Messages"));
                services.Configure<DicomSettings>(hostBuilderContext.Configuration.GetSection("dicomSettings"));
                
                //services.Configure<SomeConfig>(hostBuilderContext.Configuration.GetSection("SomeConfig"));
                //services.AddScoped(sp => sp.GetService<IOptionsSnapshot<MessagesOptions>>().Value);

            })
           .ConfigureAppConfiguration(configBuilder =>
           { 
               configBuilder.AddJsonFile("appconfig.json",false,true);
               configBuilder.Add(new MyConfigurationSource());
               configBuilder.AddEnvironmentVariables();
               configBuilder.AddCommandLine(args);
           });

            await builder.RunConsoleAsync();

            //ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //var configRoot = configurationBuilder.AddJsonFile("appconfig.json")
            //    .AddCommandLine(args)
            //    .AddEnvironmentVariables().Build();
            //ChangeToken.OnChange(configRoot.GetReloadToken,
            //    () => Console.WriteLine("Configuration changed"));

            //var section0 = configRoot.GetSection("section0");
            //var section0ch = section0.GetChildren();
            //var section0key0 = configRoot.GetSection("section0:key0");
        }
    }
}
