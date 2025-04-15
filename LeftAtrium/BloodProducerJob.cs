using Quartz;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Redis;
using System.Text.Json;

namespace LeftAtrium
{
    public class BloodProducerJob : IJob
    {
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly IRedisCacheService _cacheService;
        private readonly int _pumpCapacity = 100;
        private readonly int _atpThreshold = 5;

        public BloodProducerJob(SnapshotCache<Blood> bloodCache, IRedisCacheService cacheService)
        {
            _bloodCache = bloodCache;
            _cacheService = cacheService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await PerformMotion();

            for (int i = 0; i < _pumpCapacity; i ++)
            {
                while(_bloodCache.Queue.TryDequeue(out var blood))
                {
                    if (blood is not null)
                    {
                        var messagePublisher = GetMessagePublisher();
                        messagePublisher.SendMessage(blood);
                    }
                }
            }

            await Task.CompletedTask;
        }

        private async Task PerformMotion()
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"left-atrium-{nameof(Myocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Myocyte>(key);

                if (cell != null)
                {
                    cell.PerformMotion(_atpThreshold);

                    var serializedCell = JsonSerializer.Serialize(cell);
                    await _cacheService.SetAsync(key, serializedCell);
                }
            }
        }

        private MessagePublisher<Blood> GetMessagePublisher()
        {
            Random random = new Random();
            int randomValue = random.Next(1, 101);

            if (randomValue <= 20)
                return new MessagePublisher<Blood>("mouth");
            else if (randomValue <= 60)
                return new MessagePublisher<Blood>("small-intestine");
            else
                return new MessagePublisher<Blood>("stomach");
        }
    }
}