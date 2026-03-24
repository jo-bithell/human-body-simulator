using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MessageProducer<T>> _logger;

        public MessageProducer(string routingKey, ILogger<MessageProducer<T>> logger)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _routingKey = routingKey;
            _channel.ExchangeDeclare("not-default", ExchangeType.Direct);
            _logger = logger;
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

            _logger.LogInformation("Published message");
        }
    }
}
