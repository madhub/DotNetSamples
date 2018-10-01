using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceDemo
{
    public class ProducerConsumerDemo
    {
        static BlockingCollection<int> messages = 
            new BlockingCollection<int>(new ConcurrentBag<int>(), 10);
        static CancellationTokenSource cts = new CancellationTokenSource();
        static Random random = new Random();


        public void Run()
        {
            Task.Factory.StartNew(() => ProduceAndConsume(),cts.Token);
            Console.ReadKey();
            cts.Cancel();
        }
        static void ProduceAndConsume()
        {
           var producer = Task.Factory.StartNew(() =>
           {
               RunProducer();
           });

            var consumer = Task.Factory.StartNew(() =>
            {
                RunConsumer();

            });
            try
            {
                Task.WaitAll(new[] { producer, consumer },cts.Token);

            }catch(OperationCanceledException oce)
            {
                Console.WriteLine("OperationCanceledException");
            }
            catch (AggregateException ae)
            {
                //ae.Handle(e => true);
                Console.WriteLine("AggregateException");
            }
        }

        private static void RunConsumer()
        {
            foreach (var message in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{message}\t");
                Thread.Sleep(random.Next(1000));
            }
        }

        private static void RunProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int num = random.Next(100);
                messages.Add(num);
                Console.WriteLine($"+{num}\t");
                Thread.Sleep(random.Next(1000));
            }
        }
    }
}
