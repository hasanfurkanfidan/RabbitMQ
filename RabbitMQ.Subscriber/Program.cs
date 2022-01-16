using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.Subscriber
{
    class Program
    {
        public Program()
        {


        }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://wempyzne:cvZP5CHZYcg4aTyEWpnxgIhQL81HBjKK@goose.rmq2.cloudamqp.com/wempyzne")
            };
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();


            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;
            var routekey = "Info.#";
            channel.QueueBind(queueName, "logs-topic", routekey);

            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Logları dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);

                // File.AppendAllText("log-critical.txt", message+ "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };
        }
    }
}
