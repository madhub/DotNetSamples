using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace DicomWebClient
{
    [Serializable]
    public class ResponseDecodeException : Exception
    {
        public ResponseDecodeException(string message) : base(message)
        {
        }

        protected ResponseDecodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public static class MimeMappings
    {

        public const string MultiPartRelated = "multipart/related";
    }
     internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static async IAsyncEnumerable<byte[]> DecodeMultipartMessage(HttpResponseMessage response)
        {
            //Guard.Against.Null(response, nameof(response));
            var contentType = response.Content.Headers.ContentType;
            if (contentType.MediaType != MimeMappings.MultiPartRelated)
            {
                throw new ResponseDecodeException($"Unexpected media type {contentType.MediaType}.  Expected {MimeMappings.MultiPartRelated}");
            }

            var multipartContent = await response.Content.ReadAsMultipartAsync().ConfigureAwait(false);
            foreach (var content in multipartContent.Contents)
            {
                yield return await content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }
        public static async Task<byte[]> ToBinaryData(this HttpResponseMessage response)
        {
            //Guard.Against.Null(response, nameof(response));
            using (var memoryStream = new MemoryStream())
            {
                await foreach (var buffer in DecodeMultipartMessage(response))
                {
                    await memoryStream.WriteAsync(buffer, 0, buffer.Length);
                }
                return memoryStream.ToArray();
            }
        }
    }
}
