using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MultipartDemo.Controllers
{
    [Route("[controller]")]
    public class DcmController : Controller
    {
        private readonly IDataProvider dataProvider;

        public DcmController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var instances = dataProvider.GetInstances();
            var multiPartResult = new MultipartResult();
            foreach (var instance in instances)
            {
                multiPartResult.Add(new MultipartContent()
                {
                    ContentType = Helper.GetMimeType(Path.GetExtension(instance.GetId())),
                    FileName = instance.GetId(),
                    Stream = instance.GetData()
                });
            }
            return multiPartResult;

            //return new MultipartResult()
            //{
            //    new MultipartContent()
            //    {
            //        ContentType = "text/plain",
            //        FileName = "File.txt",
            //        Stream = new LazyForwardOnlyStream(System.IO.File.OpenRead("File.txt"))
            //    },
            //    new MultipartContent()
            //    {
            //        ContentType = "application/octet-stream",
            //        FileName = "price-difference.png",
            //        Stream = new LazyForwardOnlyStream(System.IO.File.OpenRead("price-difference.png"))
            //    },
            //    new MultipartContent()
            //    {
            //        ContentType = "application/json",
            //        FileName = "File.json",
            //        Stream = new LazyForwardOnlyStream(System.IO.File.OpenRead("File.json"))
            //    }
            //};

        }
    }
}