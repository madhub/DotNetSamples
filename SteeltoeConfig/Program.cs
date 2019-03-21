using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.PlaceholderCore;
using Steeltoe.Extensions.Configuration.RandomValue;

namespace SteeltoeConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var b1 = new ConfigurationBuilder().AddRandomValueSource().Build();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(  b1.GetValue<int>("random:int"));
            }
            Console.ReadLine();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration( cb =>
                {
                    
                })
                .UseStartup<Startup>();
    }
}
