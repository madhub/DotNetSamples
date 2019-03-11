using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
            
            HttpClient client = new HttpClient(new MyHttpClientHandler());
            client.GetAsync("https://postman-echo.com/get").GetAwaiter().GetResult();
            
            Console.ReadLine();
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
