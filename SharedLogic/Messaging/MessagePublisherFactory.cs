using SharedLogic.Models;

namespace SharedLogic.Messaging
{
    public class MessagePublisherFactory
    {
        private readonly string _organName;
        private static readonly Dictionary<string, string> OrganMessageRoutingMap = new()
        {
            { "small-intestine", "right-atrium" },
            { "mouth", "right-atrium" },
            { "stomach", "right-atrium" },
            { "lungs", "left-atrium" },
            { "right-atrium", "lungs" },
            { "left-atrium", GetLeftAtriumMessagePublisher() },
        };

        public MessagePublisherFactory(string organName)
        {
            _organName = organName;
        }

        public MessagePublisher<Blood> CreateBloodMessagePublisher()
        {
            if (OrganMessageRoutingMap.TryGetValue(_organName, out var routingKey))
            {
                return new MessagePublisher<Blood>(routingKey);
            }

            throw new NotImplementedException();
        }

        private static string GetLeftAtriumMessagePublisher()
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
