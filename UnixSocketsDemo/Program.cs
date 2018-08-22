using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnixSocketsDemo
{
    class UnixSocketsOnWindows
    {
        public static void Demo()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var task = Task.Run(() =>
            {
                StartServer(path);
            });
            // wait for server to start
            Thread.Sleep(2000);


            StartClient(path);
            Console.ReadLine();
        }
        private static void StartClient(String path)
        {

            var endPoint = new UnixDomainSocketEndPoint(path);
            try
            {
                using (var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified))
                {
                    client.Connect(endPoint);
                    Console.WriteLine($"[Client] Connected to ... ..{path}");
                    String str = String.Empty;
                    var bytes = new byte[100];
                    while (!str.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("[Client]Enter something: ");
                        var line = Console.ReadLine();
                        client.Send(Encoding.UTF8.GetBytes(line));
                        Console.Write("[Client]From Server: ");

                        int byteRecv = client.Receive(bytes);
                        str = Encoding.UTF8.GetString(bytes, 0, byteRecv);
                        Console.WriteLine(str);
                    }
                }
            }
            finally
            {
                try { File.Delete(path); }
                catch { }
            }
        }

        private static void StartServer(String path)
        {
            var endPoint = new UnixDomainSocketEndPoint(path);
            try
            {
                using (var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified))
                {
                    server.Bind(endPoint);
                    Console.WriteLine($"[Server] Listening ... ..{path}");
                    server.Listen(1);
                    using (Socket accepted = server.Accept())
                    {
                        Console.WriteLine("[Server]Connection Accepted ..." + accepted.RemoteEndPoint.ToString());
                        var bytes = new byte[100];
                        while (true)
                        {
                            int byteRecv = accepted.Receive(bytes);
                            String str = Encoding.UTF8.GetString(bytes, 0, byteRecv);
                            Console.WriteLine("[Server]Received " + str);
                            accepted.Send(Encoding.UTF8.GetBytes(str.ToUpper()));
                        }

                    }
                }
            }
            finally
            {
                try { File.Delete(path); }
                catch { }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
