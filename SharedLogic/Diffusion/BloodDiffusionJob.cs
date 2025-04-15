using Quartz;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Redis;
using System.Text.Json;

namespace SharedLogic.Diffusion
{
    public class BloodDiffusionJob<C> : IJob where C : Cell
    {
        private readonly SnapshotCache<Blood> _bloodCacheCache;
        private readonly IRedisCacheService _cacheService;
        private readonly string _projectCalledFrom;

        public BloodDiffusionJob(SnapshotCache<Blood> bloodCache, IRedisCacheService cacheService, string projectCalledFrom)
        {
            _bloodCacheCache = bloodCache;
            _cacheService = cacheService;
            _projectCalledFrom = projectCalledFrom;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            while (_bloodCacheCache.Queue.TryDequeue(out var blood))
            {
                await DiffuseNutrients(blood);
                _bloodCacheCache.Queue.Enqueue(blood);
            }

            await Task.CompletedTask;
        }

        private async Task DiffuseNutrients(Blood blood)
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"{_projectCalledFrom}-{typeof(C).Name.ToLower()}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<C>(key);

                if (cell != null)
                    cell.DiffuseNutrientsFromBlood(blood);

                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}