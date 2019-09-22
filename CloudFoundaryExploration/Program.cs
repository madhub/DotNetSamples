using System;
using System.Text;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;

namespace CloudFoundaryExploration
{
    class Program
    {
        static void Main()
        {
            InvokeFunctioAbc();
            var args = Environment.GetCommandLineArgs();           
            //RedisDemo
            bool useSslConnection = false;
            if (args.Length >= 1 )
            {
                useSslConnection = true;
            }
            
            RabbitMQDemo(useSslConnection);

            Console.ReadLine();
                       
        }

        private static void InvokeFunctioAbc()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length >= 1)
            {
                Console.WriteLine("hello");
            }

        }

        private static void RabbitMQDemo(bool useSslConnection)
        {
            
            
            string amqp = string.Empty;
            string amqpUri = "amqp://kvcyirjm:B7PuvX6EH-_kPkwB7AzQuroQk1-YvMa-@termite.rmq.cloudamqp.com/kvcyirjm";
            string secureamqpUri = "amqps://kvcyirjm:B7PuvX6EH-_kPkwB7AzQuroQk1-YvMa-@termite.rmq.cloudamqp.com/kvcyirjm";
            ConnectionFactory factory = new ConnectionFactory();
            if (useSslConnection)
            {
                Console.WriteLine("using secure connection");
                amqp = secureamqpUri;
                factory.Ssl.Enabled = true;
                
            } else
            {
                Console.WriteLine("using unsecure connection");
                amqp = amqpUri;
            }
            
            factory.Uri = new Uri(amqp);
            using (IConnection conn = factory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    string exchangeName = "someexchange";
                    string queueName = "someQueue";
                    string routingKey = "abc";
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                    channel.QueueDeclare(queueName, false, false, false, null);
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
                    string message = "";
                    if ( useSslConnection)
                    {
                        message = "Hello, world! using encrypted connection - " + DateTime.Now;
                    }
                    else
                    {
                        message = "Hello, world! - " + DateTime.Now;
                    }
                    byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;
                    
                    channel.BasicPublish(exchangeName,
                                       routingKey, props,
                                       messageBodyBytes);
                    Console.WriteLine("Message published ");
                }
            }
        }

        private static void RedisDemo()
        {
            IDistributedCache cache = new RedisCache(new RedisCacheOptions()
            {
                Configuration = "hostnam:port,password=password"
            });

            cache.SetAsync("somekey", Encoding.UTF8.GetBytes("Hello How are you")).GetAwaiter().GetResult();
            var value = cache.GetAsync("somekey").GetAwaiter().GetResult();
            Console.WriteLine(Encoding.UTF8.GetString(value));
        }
    }
}

