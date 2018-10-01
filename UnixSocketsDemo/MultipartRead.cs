using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnixSocketsDemo
{
    class MultipartRead
    {
        static async Task Main(string[] args)
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.PostAsyc("{send the request to api}");

            //var streamProvider = await response.Content.ReadAsMultipartAsync();
            //foreach (var partialContent in streamProvider.Contents)
            //{
            //    var part = await partialContent.ReadAsHttpResponseMessageAsync();
            //    var partStream = await part.Content.ReadAsStreamAsync();
            //    await streamHandler(partStream);
            //}
        }
    }
}
