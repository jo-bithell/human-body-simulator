using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace KafkaCommon
{
    public class MessageConsumer<T> : BackgroundService
    {
        private readonly SnapshotCache<T> _snapshotCache;
        private readonly string _topic;
        public MessageConsumer(SnapshotCache<T> snapshotCache, string topic)
        {
            _snapshotCache = snapshotCache;
            _topic = topic;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "biology",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            Task.Run(() => ConsumeMessage(config, stoppingToken), stoppingToken);

            return Task.CompletedTask;
        }

        private void ConsumeMessage(ConsumerConfig consumerConfig, CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumer.Subscribe(_topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = consumer.Consume(stoppingToken);

                        Console.Write($"Consumed message");

                        ProcessMessage(result, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    consumer.Close();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void ProcessMessage(ConsumeResult<Ignore, string> result, CancellationToken stoppingToken)
        {
            var message = JsonSerializer.Deserialize<T>(result.Message.Value);

            if (message == null)
                return;

            _snapshotCache.Queue.Enqueue(message);
        }
    }
}
