using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ThreadingChannelExploration
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel =  Channel.CreateBounded<string>(new BoundedChannelOptions(10));
            WriteToChannel(channel.Writer);
            ReadFromChannel(channel.Reader);
            Console.ReadLine();
        }

        private static void ReadFromChannel(ChannelReader<string> reader)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Console.WriteLine("[reader] reading from channel");
                    var result = await reader.ReadAsync();
                    Console.WriteLine("[reader] read "+result);
                    await Task.Delay(3000);
                }
            });
        }

        private static  void WriteToChannel(ChannelWriter<string> writer)
        {
            Task.Factory.StartNew( async () =>
            {
                while(true)
                {
                    Console.WriteLine("[writer] writing to channel");
                    await writer.WriteAsync(DateTime.Now.ToString());
                    //Console.WriteLine("[writer] wrote something to channel , waiting for 3 sec");
                    //await Task.Delay(3000);
                }
            });
        }

        
    }
}
