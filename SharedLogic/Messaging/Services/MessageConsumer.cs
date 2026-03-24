using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLogic.Messaging.Models;
using System.Text;
using System.Text.Json;

namespace SharedLogic.Messaging.Services
{
    public class MessageConsumer<T> : BackgroundService
    {
        private readonly SnapshotCache<T> _snapshotCache;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<MessageConsumer<T>> _logger;

        public MessageConsumer(SnapshotCache<T> snapshotCache, string queueName, ILogger<MessageConsumer<T>> logger)
        {
            _snapshotCache = snapshotCache;
            _queueName = queueName;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Consumed message");

                ProcessMessage(message, stoppingToken);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void ProcessMessage(string message, CancellationToken stoppingToken)
        {
            var deserializedMessage = JsonSerializer.Deserialize<T>(message);

            if (deserializedMessage == null)
                return;

            _snapshotCache.Queue.Enqueue(deserializedMessage);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
