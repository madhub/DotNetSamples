using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore20HttpClientFactoryDemo
{
    /// <summary>
    /// Typed Http Client factory demo
    /// https://www.stevejgordon.co.uk/httpclientfactory-named-typed-clients-aspnetcore
    /// </summary>
    /// 
    public class FhirCdrHttpClient 
    {
        public FhirCdrHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }
    }
    public class DemoService : BackgroundService
    {
        private readonly FhirCdrHttpClient fhirCdrHttpClient;

        public DemoService(FhirCdrHttpClient fhirCdrHttpClient)
        {
            this.fhirCdrHttpClient = fhirCdrHttpClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = fhirCdrHttpClient.HttpClient.GetStringAsync("/baseDstu3/Patient?_pretty=true");
            Console.WriteLine(result.GetAwaiter().GetResult());
            return Task.CompletedTask;
        }
    }

    public class DemoBackgroundService : BackgroundService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DemoBackgroundService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = httpClientFactory.CreateClient("GitHubClient");
            var result = client.GetStringAsync("/");
            Console.WriteLine(result.GetAwaiter().GetResult());

            return Task.CompletedTask;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder().
                ConfigureServices((hostBuilderContext, serviceCollection) =>
                {

                    var policy = Policy.Handle<HttpRequestException>().RetryAsync(2, (e, count) =>
                    {
                        Console.WriteLine($"Retry Hanldler {count}");
                    });

                    serviceCollection.AddHttpClient("GitHubClient", client =>
                    {
                        client.BaseAddress = new Uri("https://api.github.com/");
                        client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                        client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
                    }).AddPolicyHandler(policy.AsAsyncPolicy<HttpResponseMessage>());

                    serviceCollection.AddHttpClient<FhirCdrHttpClient>(client =>
                    {
                        client.BaseAddress = new Uri(" http://hapi.fhir.org");
                        client.DefaultRequestHeaders.Add("Accept", " application/fhir+json;q=1.0");
                        client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
                    });

                   // serviceCollection.AddHostedService<DemoBackgroundService>();
                    serviceCollection.AddHostedService<DemoService>();

                }).Build();

            await host.RunAsync();
        }
    }
}
