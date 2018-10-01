using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiPartClient
{
    class Program
    {
        static  void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response =  client.GetAsync("http://localhost:53828/dcm").Result;
            if (response.Content.IsMimeMultipartContent())
            {

                //var streamProvider = response.Content.ReadAsMultipartAsync().GetAwaiter().GetResult();
                var multiPartProvider = response.Content.ReadAsMultipartAsync();
                var httpContents = multiPartProvider.Result.Contents;
                foreach (var item in httpContents)
                {
                    Console.WriteLine(item.Headers.ContentType);
                    String tempFileName = Path.GetTempFileName();
                    using (var fileStream = File.Create(tempFileName))
                    {
                        item.ReadAsStreamAsync().Result.CopyTo(fileStream);
                        Console.WriteLine($"File Created at {tempFileName}");
                    }
                      
                }
            }
            Console.ReadLine();
        }
    }
}
