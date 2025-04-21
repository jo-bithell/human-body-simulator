namespace SharedLogic.Messaging.Services.Interfaces
{
    public interface IMessageProducer<T>
    {
        void SendMessage(T message);
    }
}