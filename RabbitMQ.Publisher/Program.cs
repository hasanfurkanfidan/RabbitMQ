using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://wempyzne:cvZP5CHZYcg4aTyEWpnxgIhQL81HBjKK@goose.rmq2.cloudamqp.com/wempyzne")
            };
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.QueueDeclare("Hello-Queue", true, false, false);
            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 4);
                var random = new Random();
                var log1 = (LogNames)random.Next(1, 3);
                var log2 = (LogNames)random.Next(1, 3);
                var log3 = (LogNames)random.Next(1, 3);

                var routeKey = $"{log1}.{log2}.{log3}";
                var message = $"log-type : {log1}-{log2}-{log3}";

                //Rabbit MQ ya mesajları byte dizin olarak atıyoruz

                var messageBody = Encoding.UTF8.GetBytes(message);

               

                //Default Exchange
                channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                Console.WriteLine($"Log Gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }
}
