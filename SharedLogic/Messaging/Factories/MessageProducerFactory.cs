using Microsoft.Extensions.Logging;
using SharedLogic.Messaging.Factories.Interfaces;
using SharedLogic.Messaging.Services;
using SharedLogic.Messaging.Services.Interfaces;
using SharedLogic.Models;

namespace SharedLogic.Messaging.Factories
{
    public class MessageProducerFactory : IMessageProducerFactory
    {
        private readonly string _organName;
        private static readonly Dictionary<string, string> OrganMessageRoutingMap = new()
        {
            { "small-intestine", "right-atrium" },
            { "mouth", "right-atrium" },
            { "stomach", "right-atrium" },
            { "lungs", "left-atrium" },
            { "right-atrium", "lungs" },
            { "left-atrium", GetLeftAtriumMessageProducer() },
        };
        private readonly ILogger<MessageProducer<Blood>> _logger;

        public MessageProducerFactory(string organName, ILogger<MessageProducer<Blood>> logger)
        {
            _organName = organName;
            _logger = logger;
        }

        public IMessageProducer<Blood> CreateBloodMessageProducer()
        {
            if (OrganMessageRoutingMap.TryGetValue(_organName, out var routingKey))
            {
                return new MessageProducer<Blood>(routingKey, _logger);
            }

            throw new NotImplementedException();
        }

        private static string GetLeftAtriumMessageProducer()
        {
            Random random = new Random();
            int randomValue = random.Next(1, 101);

            if (randomValue <= 20)
                return "mouth";
            else if (randomValue <= 60)
                return "small-intestine";
            else
                return "stomach";
        }
    }
}
