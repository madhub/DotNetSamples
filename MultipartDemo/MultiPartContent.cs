using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MultipartDemo
{
    public class MultipartContent
    {
        public string ContentType { get; set; }

        public string FileName { get; set; }

        public Stream Stream { get; set; }
    }

    public class MultipartResult : Collection<MultipartContent>, IActionResult
    {
        private readonly System.Net.Http.MultipartContent multiPartContent;

        public MultipartResult(string subtype = "byteranges", string boundary = null)
        {
            if (boundary == null)
            {
                this.multiPartContent = new System.Net.Http.MultipartContent(subtype);
            }
            else
            {
                this.multiPartContent = new System.Net.Http.MultipartContent(subtype, boundary);
            }
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var item in this)
            {
                if (item.Stream != null)
                {
                    var streamContent = new StreamContent(item.Stream);

                    if (item.ContentType != null)
                    {
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);
                    }

                    if (item.FileName != null)
                    {
                        //var contentDisposition = new ContentDispositionHeaderValue("attachment")
                        //{
                        //    FileName = item.FileName
                        //};

                        //streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        //{
                        //    FileName = contentDisposition.FileName,
                        //    FileNameStar = contentDisposition.FileNameStar
                        //};


                        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = item.FileName
                        };
                    }

                    this.multiPartContent.Add(streamContent);
                }
            }

            //context.HttpContext.Response.ContentLength = multiPartContent.Headers.ContentLength;
            context.HttpContext.Response.ContentType = multiPartContent.Headers.ContentType.ToString();

            await multiPartContent.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
