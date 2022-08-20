using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using System.Collections;
using System.Text;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Demofunction
{
    /// <summary>
    /// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Functions()
        {
        }

        ///// <summary>
        ///// Root route that provides information about the other requests that can be made.
        /////
        ///// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        ///// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        ///// default to Zip.
        ///// </summary>
        ///// <returns>API descriptions.</returns>
        //[LambdaFunction()]
        //[HttpApi(LambdaHttpMethod.Get, "/")]
        //public string Default()
        //{
        //    var docs = @"Lambda Calculator Home:
        //                You can make the following requests to invoke other Lambda functions perform calculator operations:
        //                /add/{x}/{y}
        //                /substract/{x}/{y}
        //                /multiply/{x}/{y}
        //                /divide/{x}/{y}
        //                ";
        //    return docs;
        //}

        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/env")]
        public string Test([FromQuery]Dictionary<string,string> queryParams, [FromHeader] Dictionary<string, string> headers)
        {
           // var queryStr = queryParams.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b);
           // var headerStr = headers.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b);
            IDictionary nonGenDict = Environment.GetEnvironmentVariables();
            Dictionary<string, string> genDict = new Dictionary<string, string>();
            var enumerator = nonGenDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                genDict[enumerator.Key.ToString()] = (string)enumerator.Value;
            }
                       
            
            Dictionary<string, Dictionary<string, string>> response = new Dictionary<string, Dictionary<string, string>>();
            response.Add("headers",headers);
            response.Add("queryParams", queryParams);
            response.Add("env", genDict);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(response, options);

            return jsonString;
        }
        ///// <summary>
        ///// Perform x + y
        /////
        ///// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        ///// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        ///// default to Zip.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns>Sum of x and y.</returns>
        //[LambdaFunction()]
        //[HttpApi(LambdaHttpMethod.Get, "/add/{x}/{y}")]
        //public int Add(int x, int y, ILambdaContext context)
        //{
        //    context.Logger.LogInformation($"{x} plus {y} is {x + y}");
        //    return x + y;
        //}

        ///// <summary>
        ///// Perform x - y.
        /////
        ///// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        ///// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        ///// default to Zip.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns>x substract y</returns>
        //[LambdaFunction()]
        //[HttpApi(LambdaHttpMethod.Get, "/substract/{x}/{y}")]
        //public int Substract(int x, int y, ILambdaContext context)
        //{
        //    context.Logger.LogInformation($"{x} substract {y} is {x - y}");
        //    return x - y;
        //}

        ///// <summary>
        ///// Perform x * y.
        /////
        ///// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        ///// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        ///// default to Zip.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns>x multiply y</returns>
        //[LambdaFunction()]
        //[HttpApi(LambdaHttpMethod.Get, "/multiply/{x}/{y}")]
        //public int Multiply(int x, int y, ILambdaContext context)
        //{
        //    context.Logger.LogInformation($"{x} multiply {y} is {x * y}");
        //    return x * y;
        //}

        ///// <summary>
        ///// Perform x / y.
        /////
        ///// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        ///// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        ///// default to Zip.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns>x divide y</returns>
        //[LambdaFunction()]
        //[HttpApi(LambdaHttpMethod.Get, "/divide/{x}/{y}")]
        //public int Divide(int x, int y, ILambdaContext context)
        //{
        //    context.Logger.LogInformation($"{x} divide {y} is {x / y}");
        //    return x / y;
        //}
    }
}
