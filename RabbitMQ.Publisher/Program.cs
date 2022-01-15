using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Publisher
{
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

            channel.QueueDeclare("Hello-Queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
             {
                 var message = $"Message {x}";

                 //Rabbit MQ ya mesajları byte dizin olarak atıyoruz

                 var messageBody = Encoding.UTF8.GetBytes(message);

                 //Default Exchange
                 channel.BasicPublish(string.Empty, "Hello-Queue", null, messageBody);

                 Console.WriteLine($"Mesaj Gönderilmiştir : {message}");
             });

            Console.ReadLine();
        }
    }
}
