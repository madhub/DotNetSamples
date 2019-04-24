using System;
using System.Text;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;

namespace CloudFoundaryExploration
{

    class Program
    {
        static void Main(string[] args)
        {
            //RedisDemo();

            RabbitMQDemo();

            Console.ReadLine();

            Console.WriteLine("Hello World!");
        }

        private static void RabbitMQDemo()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://user:password@hostname/user");
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
                    byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;
                    channel.BasicPublish(exchangeName,
                                       routingKey, props,
                                       messageBodyBytes);
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
