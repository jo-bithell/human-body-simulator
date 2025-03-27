using Confluent.Kafka;
using System.Text.Json;

namespace KafkaCommon
{
    public class MessagePublisher<T>
    {
        private IProducer<string, string>? _producer;
        private string _topic;

        public MessagePublisher(string topic)
        {
            if (_producer == null)
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                };

                _producer = new ProducerBuilder<string, string>(config).Build();
            }

            _topic = topic;
        }

        public async Task SendMessage(T message)
        {
            await Publish(message);
        }

        private async Task Publish(T message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<string, string>
            {
                Value = serializedMessage
            };

            if (_producer != null)
            {
                await _producer.ProduceAsync(_topic, kafkaMessage);
            }
        }
    }
}
