using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SyncAsyncApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncAsyncController : ControllerBase
    {
        public SyncAsyncController()
        {

        }
        [HttpGet("sync")]
        public IActionResult SyncGet()
        {
            List<string> environment= new List<string>();
            foreach( DictionaryEntry dictionaryEntry in Environment.GetEnvironmentVariables())
            {
                environment.Add($"{dictionaryEntry.Key} = {dictionaryEntry.Value}");
            }
            Thread.Sleep(1000);

            return Ok(environment);
        }

        [HttpGet("async")]
        public async Task<IActionResult> AsyncGet()
        {

            List<string> environment = new List<string>();
            foreach (DictionaryEntry dictionaryEntry in Environment.GetEnvironmentVariables())
            {
                environment.Add($"{dictionaryEntry.Key} = {dictionaryEntry.Value}");
            }
            await Task.Delay(1000);
            return Ok(environment);
        }
    }
}