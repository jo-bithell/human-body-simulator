using KafkaCommon;
using SharedLogic.Models;
using SharedLogic.Workers;

namespace SharedLogic
{
    public class BloodProducerWorker : BaseWorker
    {
        private readonly MessagePublisher<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCacheCache;

        public BloodProducerWorker(MessagePublisher<Blood> producerService, SnapshotCache<Blood> bloodCache)
        {
            _producerService = producerService;
            _bloodCacheCache = bloodCache;
        }

        public override async Task PerformAction(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_bloodCacheCache.Queue.TryDequeue(out var blood))
                {
                    await _producerService.SendMessage(blood);
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}