using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProducerConsumerDemoUsingChannels
{
    class Program
    {
        static void Main(string[] args)
        {
            String directoryToProces = @"C:\app";
            int numberOfReaders = 10;
            string[] fileEntries = Directory.GetFiles(directoryToProces);
            var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(10));

            // create cancellation token to cancel processing activity
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            List<Task> allTasks = new List<Task>();
            // create producer
            var writerTask = WriteToChannel(channel.Writer, fileEntries, cancellationTokenSource.Token);
            allTasks.Add(writerTask);

            // create consumers
            var readerTasks = ReadFromChannel(channel.Reader, cancellationTokenSource.Token, numberOfReaders);
            allTasks.AddRange(readerTasks);
            Console.WriteLine($"Process id {Process.GetCurrentProcess().Id}");
            Console.WriteLine("Enter to quit");
            Console.ReadLine();

            // cancel ongoing task
            cancellationTokenSource.Cancel();
            // wait for all task to complete
            Task.WhenAll(allTasks);
        }

        /// <summary>
        /// Consume work from multiple threads
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<Task> ReadFromChannel(ChannelReader<string> reader,CancellationToken token,int numberOfReaders=1)
        {
            Console.WriteLine($"[ReadFromChannel] Creating {numberOfReaders} task to consume work ...");
            List<Task> readers = new List<Task>();
            for (int index = 0; index < numberOfReaders; index++)
            {
                var newTask = Task.Factory.StartNew(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        Console.WriteLine($"[{Process.GetCurrentProcess().Id}][ReadFromChannel] {Thread.CurrentThread.ManagedThreadId} reading from channel");
                        var result = await reader.ReadAsync(token);
                        Console.WriteLine($"[{Process.GetCurrentProcess().Id}][ReadFromChannel] read " + result);
                        // simulating processing
                        await Task.Delay(500,token);
                    }
                },token);
                readers.Add(newTask);
            }
            
            return readers;
        }
        /// <summary>
        /// Produce work
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="fileEntries"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static Task WriteToChannel(ChannelWriter<string> writer, string[] fileEntries, CancellationToken token)
        {
            Console.WriteLine($"[WriteToChannel] Producing work ...");
            var newTask = Task.Factory.StartNew(async () =>
            {
                int fileIndex = 0;
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine($"[{Process.GetCurrentProcess().Id}][WriteToChannel]{Thread.CurrentThread.ManagedThreadId} processing file {fileEntries[fileIndex]}");
                    await writer.WriteAsync(fileEntries[fileIndex],token);
                    if ( fileIndex < (fileEntries.Length - 1))
                    {
                        fileIndex++;
                    }
                    else
                    {
                        fileIndex = 0;
                    }
                    
                }
            },token);
            return newTask;
        }
    }
}
