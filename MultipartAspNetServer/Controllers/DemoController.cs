using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

//echo -ne "\r\n--myboundary\r\nContent-Type: application/dicom\r\n\r\n" > mime.head
//echo -ne "\r\n--myboundary--" > mime.tail
//cat mime.head dicom-file1.dcm mime.head dicom-file2.dcm mime.tail > study.dcm
//curl -X POST -H "Content-Type: multipart/related; type=\"application/dicom\"; boundary=myboundary" \
//     http://localhost:8080/dcm4chee-arc/aets/DCM4CHEE/rs/studies --data-binary @study.dcm

    //curl -X POST -H "Content-Type: multipart/related; type=\"application/dicom\"; boundary=myboundary" https://localhost:5001/api/demo --data-binary @file.pdf
namespace MultipartAspNetServer.Controllers
{
    public interface IStoreProvider
    {
        Task StoreAsync(Stream stream);
    }
    public class FileStore : IStoreProvider
    {
        private static int MAX_ARRAY_SIZE = 1024*1024*5;

        public async Task StoreAsync(Stream stream)
        {
            var samePool = ArrayPool<byte>.Shared;
            byte[] buffer = samePool.Rent(MAX_ARRAY_SIZE);
            try
            {
                while (true)
                {
                    var bytesRead = await stream.ReadAsync(buffer);
                    if (bytesRead == 0)
                    {
                        // read all bytes
                        break;
                    }

                    // check header, if invalid reject it-
                }

            }
            finally
            {
                samePool.Return(buffer);

            }
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // https://localhost:5001/api/demo?search="><script>alert(0)</script>
        // https://localhost:5001/api/demo?search=%3Cscript%3Ealert%280%29%3C%2Fscript%3E
        // GET api/demo?search=searchTerm
        public ActionResult<string> Get(string search)
        {
            Console.WriteLine();
            return "value";
        }

        [HttpPost]
        
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {

            IStoreProvider store = new FileStore();
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType),
                            int.MaxValue);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);
                if (section.ContentType.Equals("application/dicom"))
                {
                        //await section.Body.CopyToAsync(targetStream);
                   await store.StoreAsync(section.Body);
                    

                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();

            }
            return new JsonResult("") ;
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }

    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (string.IsNullOrWhiteSpace(boundary.Value))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.Value;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                   && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                       || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}