using System;
using System.Diagnostics.Tracing;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SslRemoteCertDump
{
    class Program
    {
        private const int SSLPort = 443;

        static async Task Main(string[] args)
        {
            await ConnectToServer(args[0]);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

        }
        public static async Task ConnectToServer(String targetHost)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                Console.WriteLine($"Connecting to {targetHost}:{SSLPort} ...");
                await tcpClient.ConnectAsync(targetHost, SSLPort);

                using (SslStream sslStream = new SslStream(tcpClient.GetStream()))
                {
                    await sslStream.AuthenticateAsClientAsync(targetHost);

                    await Console.Out.WriteLineAsync($"Connected to {targetHost} with {sslStream.SslProtocol}");

                    await Console.Out.WriteLineAsync($"IsMutuallyAuthenticated = {sslStream.IsMutuallyAuthenticated}");
                    await Console.Out.WriteLineAsync($"IsAuthenticated = {sslStream.IsAuthenticated}");
                    await Console.Out.WriteLineAsync($"CipherAlgorithm = {sslStream.CipherAlgorithm}");
                    await Console.Out.WriteLineAsync($"CipherStrength  = {sslStream.CipherStrength }");
                    await Console.Out.WriteLineAsync($"HashAlgorithm  = {sslStream.HashAlgorithm }");
                    await Console.Out.WriteLineAsync($"KeyExchangeAlgorithm  = {sslStream.KeyExchangeAlgorithm }");
                    await Console.Out.WriteLineAsync($"HashStrength  = {sslStream.HashStrength }");
                    await Console.Out.WriteLineAsync($"KeyExchangeStrength  = {sslStream.KeyExchangeStrength }");
                    await Console.Out.WriteLineAsync($"RemoteCertificate  {sslStream.RemoteCertificate.ToString(true)}");
                }
            }
        }
    }
}
