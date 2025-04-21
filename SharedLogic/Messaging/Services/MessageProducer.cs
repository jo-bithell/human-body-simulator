using RabbitMQ.Client;
using SharedLogic.Messaging.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace SharedLogic.Messaging.Services
{
    public class MessageProducer<T> : IMessageProducer<T>
    {
        private IModel _channel;
        private string _routingKey;

        public MessageProducer(string routingKey)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _routingKey = routingKey;
            _channel.ExchangeDeclare("not-default", ExchangeType.Direct);
        }

        public void SendMessage(T message)
        {
            Publish(message);
        }

        private void Publish(T message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            _channel.BasicPublish(string.Empty, routingKey: _routingKey, body: body);

            Console.WriteLine($"Published message");
        }
    }
}
