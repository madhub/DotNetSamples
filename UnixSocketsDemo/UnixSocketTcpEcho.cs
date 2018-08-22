using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnixSocketsDemo
{

    //class UnixSocketTcpEcho
    //{
    //    static async Task Main(string[] args)
    //    {
    //        if (args.Length > 0)
    //        {
    //            string filePath = args[0];
    //            Console.WriteLine($"Connecting unix socket @ {filePath}");
    //            await ConnectToServer(filePath);
    //        }
    //        else
    //        {
    //            await StartServer();
    //        }
            
    //    }

    //    private static async Task ConnectToServer(string filePath)
    //    {
    //        var endPoint = new UnixDomainSocketEndPoint(filePath);
    //        using (var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified))
    //        {
    //            await client.ConnectAsync(endPoint);
                
               
    //        }
                
            


    //    }

    //    private static async Task StartServer()
    //    {
    //        string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    //        var endPoint = new UnixDomainSocketEndPoint(path);

    //        var listenSocket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);// new Socket(SocketType.Stream, ProtocolType.Tcp);
    //        //listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8087));
    //        listenSocket.Bind(endPoint);
    //        Console.WriteLine($"Listening on unix socket @{path}");

    //        listenSocket.Listen(120);

    //        while (true)
    //        {
    //            var socket = await listenSocket.AcceptAsync();
    //            _ = ProcessLinesAsync(socket);
    //        }
    //    }

    //    private static async Task ProcessLinesAsync(Socket socket)
    //    {
    //        Console.WriteLine($"[{socket.RemoteEndPoint}]: connected");

    //        var pipe = new Pipe();
    //        Task writing = FillPipeAsync(socket, pipe.Writer);
    //        Task reading = ReadPipeAsync(socket, pipe.Reader);

    //        await Task.WhenAll(reading, writing);

    //        Console.WriteLine($"[{socket.RemoteEndPoint}]: disconnected");
    //    }

    //    private static async Task FillPipeAsync(Socket socket, PipeWriter writer)
    //    {
    //        const int minimumBufferSize = 512;

    //        while (true)
    //        {
    //            try
    //            {
    //                // Request a minimum of 512 bytes from the PipeWriter
    //                Memory<byte> memory = writer.GetMemory(minimumBufferSize);

    //                int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
    //                if (bytesRead == 0)
    //                {
    //                    break;
    //                }

    //                // Tell the PipeWriter how much was read
    //                writer.Advance(bytesRead);
    //            }
    //            catch
    //            {
    //                break;
    //            }

    //            // Make the data available to the PipeReader
    //            FlushResult result = await writer.FlushAsync();

    //            if (result.IsCompleted)
    //            {
    //                break;
    //            }
    //        }

    //        // Signal to the reader that we're done writing
    //        writer.Complete();
    //    }

    //    private static async Task ReadPipeAsync(Socket socket, PipeReader reader)
    //    {
    //        while (true)
    //        {
    //            ReadResult result = await reader.ReadAsync();

    //            ReadOnlySequence<byte> buffer = result.Buffer;
    //            SequencePosition? position = null;

    //            do
    //            {
    //                // Find the EOL
    //                position = buffer.PositionOf((byte)'\n');

    //                if (position != null)
    //                {
    //                    var line = buffer.Slice(0, position.Value);
    //                    ProcessLine(socket, line);

    //                    // This is equivalent to position + 1
    //                    var next = buffer.GetPosition(1, position.Value);

    //                    // Skip what we've already processed including \n
    //                    buffer = buffer.Slice(next);
    //                }
    //            }
    //            while (position != null);

    //            // We sliced the buffer until no more data could be processed
    //            // Tell the PipeReader how much we consumed and how much we left to process
    //            reader.AdvanceTo(buffer.Start, buffer.End);

    //            if (result.IsCompleted)
    //            {
    //                break;
    //            }
    //        }

    //        reader.Complete();
    //    }

    //    private static void ProcessLine(Socket socket, in ReadOnlySequence<byte> buffer)
    //    {
    //        Console.Write($"[{socket.RemoteEndPoint}]: ");
    //        foreach (var segment in buffer)
    //        {

    //            Console.Write(Encoding.UTF8.GetString(segment.Span));
    //        }
    //        Console.WriteLine();
    //    }
    //}

}

