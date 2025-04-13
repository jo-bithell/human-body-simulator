using Quartz;
using SharedLogic.Messaging;
using SharedLogic.Models;

namespace SharedLogic
{
    public class BloodProducerJob : IJob
    {
        private readonly MessagePublisher<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCacheCache;

        public BloodProducerJob(MessagePublisher<Blood> producerService, SnapshotCache<Blood> bloodCache)
        {
            _producerService = producerService;
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