using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Services;
using System.Text.Json;

namespace Lungs
{
    internal class BreathingWorker : IJob
    {
        private readonly int _atpThreshold = 5;
        private readonly IRedisCacheService _cacheService;
        private readonly Air _air;

        public BreathingWorker(IRedisCacheService cacheService, Air air)
        {
            _cacheService = cacheService;
            _air = air;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Pretend this intakes oxygen
            await PerformMotion();
            RefreshAirInLungs();
            Console.WriteLine("Blood oxygenated");

            await Task.CompletedTask;
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

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}