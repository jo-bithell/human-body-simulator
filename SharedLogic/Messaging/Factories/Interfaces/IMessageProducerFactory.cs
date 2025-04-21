using SharedLogic.Messaging.Services.Interfaces;
using SharedLogic.Models;

namespace SharedLogic.Messaging.Factories.Interfaces
{
    public interface IMessageProducerFactory
    {
        IMessageProducer<Blood> CreateBloodMessageProducer();

    }
}