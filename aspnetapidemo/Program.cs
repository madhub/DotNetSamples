using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace aspnetapidemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (Environment.GetEnvironmentVariable("VCAP_APPLICATION") != null)
            {
                Console.WriteLine($"Listening on ... http://*:{Environment.GetEnvironmentVariable("PORT")}");
                return WebHost.CreateDefaultBuilder(args)
                    .UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT")}")
                    .UseStartup<Startup>();
            }
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
