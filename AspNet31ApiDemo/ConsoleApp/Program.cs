using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class MyHandler : HttpClientHandler
    {
        public MyHandler()
        {

        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            return base.SendAsync(request, cancellationToken);
        }

    }
    public class MyHttpClientFactory : HttpClientFactory
    {
        private static HttpClient httpClient = new HttpClient(new MyHandler());
        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            return  httpClient;
        }
    }
    internal class Program
    {
        public static void Retry( Action retryAction,int retryCount)
        {
            while (true)
            {
                try
                {
                    retryAction();
                }
                catch when (retryCount-- > 0) { }
            }
        }

        static void Main(string[] args)
        {
            Retry(() =>
           {


           }, 10);
        }
        private const string accessKey = "minioadmin";
        private const string secretKey = "minioadmin"; // do not store secret key hardcoded in your production source code!
        static void Main123(string[] args)
        {
            var trailingHeaders = new Dictionary<string, string>
            {
                {"x-amz-checksum-sha256", "" }  // checksum will be calculated as the stream is read then replaced at the end
            };
            TrailingHeadersWrapperStream trailingHeadersWrapperStream =
                new TrailingHeadersWrapperStream(File.OpenRead(@"C:\dev\dotnetprojects\aspcore31demo\bin\Debug\netcoreapp3.1\appsettings.json"), trailingHeaders, CoreChecksumAlgorithm.SHA256);

            StreamReader streamReader = new StreamReader(trailingHeadersWrapperStream);
            Console.WriteLine(streamReader.ReadToEnd());

            //Task.Run(MainAsync).GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
                 
            var config = new AmazonS3Config
            {
                AuthenticationRegion = RegionEndpoint.USEast1.SystemName, // Should match the `MINIO_REGION` environment variable.
                ServiceURL = "http://127.0.0.1:9000", // replace http://localhost:9000 with URL of your MinIO server
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            config.HttpClientFactory = new MyHttpClientFactory();
            var amazonS3Client = new AmazonS3Client(accessKey, secretKey, config);
            TransferUtility utility = new TransferUtility(amazonS3Client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            request.ChecksumAlgorithm = ChecksumAlgorithm.SHA256;



            request.BucketName = "demo";
            
            request.Key = "myobject"; //file name up in S3  
            request.InputStream = File.OpenRead(@"C:\dev\dotnetprojects\aspcore31demo\bin\Debug\netcoreapp3.1\appsettings.json");
            utility.Upload(request); //commensing the transfer 

            //// uncomment the following line if you like to troubleshoot communication with S3 storage and implement private void OnAmazonS3Exception(object sender, Amazon.Runtime.ExceptionEventArgs e)
            //// amazonS3Client.ExceptionEvent += OnAmazonS3Exception;

            //var listBucketResponse = await amazonS3Client.ListBucketsAsync();

            //foreach (var bucket in listBucketResponse.Buckets)
            //{
            //    Console.Out.WriteLine("bucket '" + bucket.BucketName + "' created at " + bucket.CreationDate);
            //}
            //if (listBucketResponse.Buckets.Count > 0)
            //{
            //    var bucketName = listBucketResponse.Buckets[0].BucketName;

            //    var listObjectsResponse = await amazonS3Client.ListObjectsAsync(bucketName);

            //    foreach (var obj in listObjectsResponse.S3Objects)
            //    {
            //        Console.Out.WriteLine("key = '" + obj.Key + "' | size = " + obj.Size + " | tags = '" + obj.ETag + "' | modified = " + obj.LastModified);
            //    }
            //}
        }
    }
}
