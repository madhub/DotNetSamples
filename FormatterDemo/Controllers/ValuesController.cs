using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FormatterDemo.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet("")]
        public IActionResult Get(string id)
        {
            return new ObjectResult(Environment.GetEnvironmentVariables());
        }

        
    }
}
