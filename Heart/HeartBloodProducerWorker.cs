using Quartz;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Services;
using System.Text.Json;

namespace Heart
{
    public class HeartBloodProducerWorker : IJob
    {
        private readonly MessagePublisher<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly IRedisCacheService _cacheService;
        private readonly int _pumpCapacity = 100;
        private readonly int _atpThreshold = 5;

        public HeartBloodProducerWorker(MessagePublisher<Blood> producerService, SnapshotCache<Blood> bloodCache, IRedisCacheService cacheService)
        {
            _producerService = producerService;
            _bloodCache = bloodCache;
            _cacheService = cacheService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            PopulateBloodCache();
            await PerformMotion();

            for (int i = 0; i < _pumpCapacity; i ++)
            {
                while(_bloodCache.Queue.TryDequeue(out var blood))
                {
                    if (blood is not null)
                        _producerService.SendMessage(blood);
                }
            }

            await Task.CompletedTask;
        }

        // TODO: do this on startup
        private void PopulateBloodCache()
        {
            for (int i = 0; i < 5; i++)
            {
                Blood newBlood = new Blood();
                _bloodCache.Queue.Enqueue(newBlood);
            }
        }

        private async Task PerformMotion()
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"lungs-{nameof(Myocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Myocyte>(key);

                if (cell != null)
                    cell.PerformMotion(_atpThreshold);

                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}