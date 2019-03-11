
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurentDemo
{

    class Program
    {

        // add your code here decompress here
        public static void ProcessFile(String fileToProcess)
        {

            // create decompressor object
            // decompress image
            // log message success/failure message
            Thread.Sleep(50);
            // close decompresss object

        }


        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage : Demo <directory_to_process> <number_of_tasks>");
                return;
            }
            String directoryToProcees = args[0];
            int numberOfTask = Convert.ToInt32(args[1]);
            string[] fileEntries = Directory.GetFiles(directoryToProcees);

            // create cancellation token
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            FileLogger fileLogger = new FileLogger(cancellationTokenSource.Token);
            fileLogger.StartLoggerService();
            Console.WriteLine($"Processing directory {directoryToProcees} with {numberOfTask} task. Total files found {fileEntries.Length}");

            // blocking collection to enqueue tasks, I have put 5000 items has a capacity
            var bc = new BlockingCollection<String>(5000);

            // list to track all tasks
            List<Task> alltasks = new List<Task>();

            // enqueue files for processing
            Task fileEnquerTask = EnqueFilesForProcessing(fileEntries, bc, cancellationTokenSource.Token);
            alltasks.Add(fileEnquerTask);

            // dequeue files for processing
            var tasksCreated = DequeueFilesForProcessing(numberOfTask, bc, cancellationTokenSource.Token, fileLogger);
            alltasks.AddRange(tasksCreated);

            // wait for all enter key, & set cancellation 
            Console.WriteLine($"Processing files .... Enter to stop");
            Console.ReadLine();
            Console.WriteLine($"Cancelling all tasks...");
            // signal the cancellation 
            cancellationTokenSource.Cancel();

            // wait for all tasks to exit
            Task.WaitAll(alltasks.ToArray());

            // give time for logger thread to flush the content
            fileLogger.StopLoggerService();

            Thread.Sleep(2000);
            Console.WriteLine($"Done");
        }

        /// <summary>
        /// Enumerate files in given directory & enqueue into blocking collection
        /// same set of files will be enqueued repeatedly 
        /// </summary>
        /// <param name="fileEntries"></param>
        /// <param name="bc"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static Task EnqueFilesForProcessing(string[] fileEntries, BlockingCollection<string> bc, CancellationToken token)
        {
            // task to enqueue files 
            var fileEnquerTask = Task.Run(() =>
            {
                // repeatadely enqueq file till cancellation is requested
                int index = 0;
                while (!token.IsCancellationRequested)
                {
                    try
                    {   
                        bc.Add(fileEntries[index], token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    

                    if (index < (fileEntries.Length - 1))
                    {
                        index++;
                    }
                    else
                    {
                        // reset the the index
                        index = 0;
                    }

                }
                Console.WriteLine($"[EnqueFilesForProcessing]Cancellation Requested");
            }, token);
            return fileEnquerTask;
        }

        /// <summary>
        /// Create given number of tasks to process files queued in the blocking collection
        /// each task will dequeue file & process,return to dequeue more files till cancellation signal is received
        /// </summary>
        /// <param name="numberOfTasks"></param>
        /// <param name="bc"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="fileLogger"></param>
        /// <returns>return task array to be used for waiting</returns>
        public static Task[] DequeueFilesForProcessing(int numberOfTasks,
            BlockingCollection<String> bc,
            CancellationToken cancellationToken, FileLogger fileLogger)
        {
            var tasks = new Task[numberOfTasks];

            for (int i = 0; i < numberOfTasks; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    String fileToProcess = string.Empty;
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            fileToProcess = bc.Take(cancellationToken);
                        }
                        catch (InvalidOperationException) { }
                        catch (OperationCanceledException)
                        {
                            break;
                        }

                        if (fileToProcess != null)
                        {
                            fileLogger.LogMessage($"Processing file  {fileToProcess}");

                            Program.ProcessFile(fileToProcess);
                        }

                    }
                    Console.WriteLine($"[DequeueFilesForProcessing]Cancellation Requested");

                }, cancellationToken);
            }

            return tasks;
        }
    }

    // file logger
    public class FileLogger
    {
        private StreamWriter fileStream = null;
        private ConcurrentQueue<string> _logMessages = new ConcurrentQueue<string>();
        private readonly CancellationToken _cancellationToken;

        public FileLogger(CancellationToken cancellationToken)
        {
            this._cancellationToken = cancellationToken;
        }

        public void StartLoggerService()
        {
            fileStream = File.AppendText(Path.Combine(Environment.CurrentDirectory, "log.txt"));

            Task.Run(() =>
            {
                int lines = 0;
                while (!_cancellationToken.IsCancellationRequested)
                {
                   
                    while(true)
                    {
                        // if there are elements deque
                        if (_logMessages.TryDequeue(out string localValue))
                        {
                            fileStream.WriteLine(localValue);
                            lines++;
                        }else
                        {
                            break;
                        }
                    }
                     
                Thread.Sleep(1000);
                    // flush every 25 lines
                if (lines > 25)
                {
                     fileStream.Flush();
                        lines = 0;
                }
                
            }

            }, _cancellationToken);
        }
        public void StopLoggerService()
        {
            fileStream.Flush();
            fileStream.Close();
        }
        public void LogMessage(String message)
        {
            _logMessages.Enqueue(($"[Thread-{Thread.CurrentThread.ManagedThreadId}][{DateTime.Now}] - {message}"));
        }

    }

}
