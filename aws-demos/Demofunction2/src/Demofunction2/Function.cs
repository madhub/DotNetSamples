using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Collections;
using System.Net;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Demofunction2
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogInformation("Get Request\n");
            IDictionary nonGenDict = Environment.GetEnvironmentVariables();
            Dictionary<string, string> genDict = new Dictionary<string, string>();
            var enumerator = nonGenDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                genDict[enumerator.Key.ToString()] = (string)enumerator.Value;
            }


            Dictionary<string, IDictionary<string, string>> data = new Dictionary<string, IDictionary<string, string>>();
            data.Add("headers", request.Headers);
            data.Add("queryParams", request.QueryStringParameters);
            data.Add("pathParams", request.PathParameters);
            data.Add("env", genDict);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);


            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = jsonString,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return response;
        }
    }
}