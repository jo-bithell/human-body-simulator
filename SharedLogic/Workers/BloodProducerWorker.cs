using KafkaCommon;
using Quartz;
using SharedLogic.Models;

namespace SharedLogic
{
    public class BloodProducerWorker : IJob
    {
        private readonly MessagePublisher<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCacheCache;

        public BloodProducerWorker(MessagePublisher<Blood> producerService, SnapshotCache<Blood> bloodCache)
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