using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatterDemo
{
    internal class MyDataFormatters : TextOutputFormatter
    {
        private const string Input = "application/vnd.ms+json";
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings
            = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

        public MyDataFormatters()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(Input));
            SupportedEncodings.Add(Encoding.UTF8);
        }
        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        //public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        //{

        //    return context.HttpContext.Response.WriteAsync(
        //        JsonConvert.SerializeObject(context.Object, DefaultJsonSerializerSettings),
        //        selectedEncoding,
        //        context.HttpContext.RequestAborted);
        //}
    }
}
