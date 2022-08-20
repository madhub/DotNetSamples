using Amazon.Lambda.Annotations;

namespace LambdaDemo
{
    public class Functions
    {
        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Get,"/plus/{x}/{y}")]
        public int Plus(int x, int y)
        {
            return x + y;
        }

    }
}