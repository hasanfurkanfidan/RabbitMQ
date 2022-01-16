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
            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                Random random = new Random();
                LogNames log =(LogNames) random.Next(1, 3);
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 4);
                var message = $"log-type : {log}";

                //Rabbit MQ ya mesajları byte dizin olarak atıyoruz

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                //Default Exchange
                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"Log Gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }
}
