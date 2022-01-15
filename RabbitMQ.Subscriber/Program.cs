using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Subscriber
{
    class Program
    {
        IModel _channel;
        public Program()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://wempyzne:cvZP5CHZYcg4aTyEWpnxgIhQL81HBjKK@goose.rmq2.cloudamqp.com/wempyzne")
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }
        void Main(string[] args)
        {
            _channel.QueueDeclare("Hello-Queue", true, false, false);

            var consumer = new EventingBasicConsumer(_channel);

            _channel.BasicConsume("Hello-Queue", false, consumer);

            consumer.Received += Consumer_Received;
            Console.ReadLine();
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"Gelen Mesaj:{message}");
            _channel.BasicAck(e.DeliveryTag,false);
        }
    }
}
