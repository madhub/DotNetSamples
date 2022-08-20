using Amazon.CDK;

namespace CdkSample
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new CdkSampleStack(app, "CdkSampleStack");

            app.Synth();
        }
    }
}
