using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CorsAspdemo
{
    /// <summary>
    /// CURL sample  ( Reference https://developers.digitalocean.com/documentation/v2/#cors)
    //curl -I -H "Origin: http://example.com" -H "Access-Control-Request-Method: POST" -H "Access-Control-Request-Headers: X-Requested-With"  -X OPTIONS "http://localhost:10713/api/values"
    //HTTP/1.1 204 No Content
    //Date: Sun, 22 Sep 2019 11:46:08 GMT
    //Server: Kestrel
    //Vary: Origin
    //Access-Control-Allow-Headers: X-Requested-With
    //Access-Control-Allow-Methods: GET,POST
    //Access-Control-Allow-Origin: http://example.com
    /// </summary>
    /// 

    //curl -I -H "Origin: http://www.contoso.com" -H "Access-Control-Request-Method: POST" -H "Access-Control-Request-Headers: X-Requested-With"  -X OPTIONS "http://localhost:10713/api/values"
    //HTTP/1.1 204 No Content
    //Date: Sun, 22 Sep 2019 11:45:28 GMT
    //Server: Kestrel
    //Vary: Origin
    //Access-Control-Allow-Headers: X-Requested-With
    //Access-Control-Allow-Methods: GET,POST
    //Access-Control-Allow-Origin: http://www.contoso.com
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging( logb =>
            {
                logb.AddConsole();
                logb.AddDebug();
            })
                .UseStartup<Startup>()
                .Build();
    }
}
