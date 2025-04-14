using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Services;
using System.Text.Json;

namespace Lungs
{
    internal class AirDiffusionJob : IJob
    {
        private readonly Air _air;
        private readonly IRedisCacheService _cacheService;

        public AirDiffusionJob(Air air, IRedisCacheService cacheService)
        {
            _air = air;
            _cacheService = cacheService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"lungs-{nameof(AlveolarCell)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<AlveolarCell>(key);

                if (cell != null)
                    cell.DiffuseGases(_air);

                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }

            Console.WriteLine("Air diffused into alveolar cells");
            await Task.CompletedTask;
        }
    }
}