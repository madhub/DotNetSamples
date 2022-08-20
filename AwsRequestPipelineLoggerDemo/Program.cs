using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3;
using System;
using System.Threading.Tasks;

namespace AwsRequestPipelineLoggerDemo
{
    public class DemoPiplelineHandler : PipelineHandler
    {
        public override Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            return base.InvokeAsync<T>(executionContext);
        }
    }

    public class DemoAwsClientPipelineCustomizer : IRuntimePipelineCustomizer
    {
        public string UniqueName => "demo.pipeline";

        public void Customize(Type type, RuntimePipeline pipeline)
        {
            pipeline.AddHandler(new DemoPiplelineHandler());

        }
    }
    internal class Program
    {
        private const string accessKey = "minioadmin";
        private const string secretKey = "minioadmin"; // do not store secret key hardcoded in your production source code!

        static void Main(string[] args)
        {
            // register customizer

            RuntimePipelineCustomizerRegistry.Instance.Register(new DemoAwsClientPipelineCustomizer());

            Task.Run(MainAsync).GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var config = new AmazonS3Config
            {
                AuthenticationRegion = RegionEndpoint.USEast1.SystemName, // Should match the `MINIO_REGION` environment variable.
                ServiceURL = "http://localhost:9000", // replace http://localhost:9000 with URL of your MinIO server
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            var amazonS3Client = new AmazonS3Client(accessKey, secretKey, config);

            // uncomment the following line if you like to troubleshoot communication with S3 storage and implement private void OnAmazonS3Exception(object sender, Amazon.Runtime.ExceptionEventArgs e)
            // amazonS3Client.ExceptionEvent += OnAmazonS3Exception;

            var listBucketResponse = await amazonS3Client.ListBucketsAsync();

            foreach (var bucket in listBucketResponse.Buckets)
            {
                Console.Out.WriteLine("bucket '" + bucket.BucketName + "' created at " + bucket.CreationDate);
            }
            if (listBucketResponse.Buckets.Count > 0)
            {
                var bucketName = listBucketResponse.Buckets[0].BucketName;

                var listObjectsResponse = await amazonS3Client.ListObjectsAsync(bucketName);

                foreach (var obj in listObjectsResponse.S3Objects)
                {
                    Console.Out.WriteLine("key = '" + obj.Key + "' | size = " + obj.Size + " | tags = '" + obj.ETag + "' | modified = " + obj.LastModified);
                }
            }
        }
    }
}
