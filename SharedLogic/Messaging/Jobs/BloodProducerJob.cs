using Quartz;
using SharedLogic.Messaging.Factories.Interfaces;
using SharedLogic.Messaging.Models;
using SharedLogic.Messaging.Services.Interfaces;
using SharedLogic.Models;

namespace SharedLogic
{
    public class BloodProducerJob : IJob
    {
        private readonly IMessageProducer<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCacheCache;

        public BloodProducerJob(IMessageProducerFactory messagePublisherFactory, SnapshotCache<Blood> bloodCache)
        {
            _producerService = messagePublisherFactory.CreateBloodMessageProducer();
            _bloodCacheCache = bloodCache;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            while (_bloodCacheCache.Queue.TryDequeue(out var blood))
            {
                _producerService.SendMessage(blood);
            }

            await Task.CompletedTask;
        }
    }
}