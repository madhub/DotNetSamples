using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.InteropServices;


namespace APIExploration
{
    public class MyHttpClientHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DumpHeader(request);
            var response = base.SendAsync(request, cancellationToken);
            DumpHeader(response);
            return response;
        }

        private void DumpHeader(Task<HttpResponseMessage> response)
        {
            var string1 = $"{response.Result.Version} {response.Result.StatusCode} \n";
            Console.WriteLine(string1);
        }

        public void DumpHeader(HttpRequestMessage httpRequestMessage)
        {
            var string1 = $"{httpRequestMessage.Method} {httpRequestMessage.RequestUri} {httpRequestMessage.Version}\n";
            Console.WriteLine(string1);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //var strings = new[] { "a", "b", "v" };

            //var files1 = strings.Select(astr => new FileStream(astr, FileMode.OpenOrCreate));
            //var files = Array.ConvertAll(strings, ele => new FileStream(ele,FileMode.OpenOrCreate));

            //HttpClient client = new HttpClient(new MyHttpClientHandler());
            //client.GetAsync("https://postman-echo.com/get").GetAwaiter().GetResult();
            //UrlApi_QueryHelpers_Tests();




            CreateShortViewOfByteArray();

            Console.ReadLine();
        }

        private static void CreateShortViewOfByteArray()
        {
            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var shortArray = MemoryMarshal.Cast<byte, short>(data);

            Console.WriteLine(shortArray.Length);
            unsafe
            {
                fixed (byte* pointerToFirst = &data[0])
                {
                    Console.WriteLine((int)pointerToFirst);
                    fixed (short* pointerToFirst2 = &shortArray[0])
                    {
                        Console.WriteLine((int)pointerToFirst2);
                        Console.WriteLine("hellos");
                    }
                }

            }
        }

        public static void UrlApi_QueryHelpers_Tests()
        {
            // https://www.troyhunt.com/owasp-top-10-for-net-developers-part-2/
            var val = Uri.IsWellFormedUriString("https://localhost:5001/api/demo?search= >< script > alert(0) </ script > ", UriKind.Absolute);
            var val1 = Uri.IsWellFormedUriString("?search= >< script > alert(0) </ script > ", UriKind.RelativeOrAbsolute);
            var rawurl = "https://example.com/some/path?key1=val1&key2=val2&key4=valdouble&key3=";
            var uri = new Uri(rawurl);
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);

            var query = QueryHelpers.ParseQuery(uri.Query);

            var items = query.SelectMany(x => 
                                                x.Value, 
                                                (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            IDictionary<string, string> dictionary = items.ToDictionary(pair => pair.Key, pair => pair.Value);

            QueryHelpers.AddQueryString(baseUri, dictionary);
            
            //var qb = new QueryBuilder(items);
            //qb.Add("nonce", "testingnonce");
            //qb.Add("payerId", "pyr_");

            //var fullUri = baseUri + qb.ToQueryString();

        }

        public static bool IsPrime(int number)
        {
            if ( number == 1 || number == 2)
            {
                return true;
            }
            for(int i = 3; i < number - 1;i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
    }
}
