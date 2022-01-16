using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

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
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();


            //var consumer = new EventingBasicConsumer(_channel);

            //_channel.BasicConsume("Hello-Queue", false, consumer);

            //consumer.Received += (o, eventArgs) =>
            //{
            //    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            //    Console.WriteLine($"Gelen Mesaj:{message}");
            //    _channel.BasicAck(eventArgs.DeliveryTag, false);
            //};

            //var randomQueueName = "logs-database-save";
            //channel.QueueDeclare(randomQueueName, true, false, false);
            //channel.QueueBind(randomQueueName, "logs-fanout", string.Empty, null);
            var queueName = "direct-queue-Critical";
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(0, 1, false);
            channel.BasicConsume(queueName, false, consumer);
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Gelen Mesaj:{message}");
                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();
        }
    }
}
