using SharedLogic.Models.Cells;
using SharedLogic.Models;
using Lungs.Services.Interfaces;
using Lungs.Models;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace Lungs.Services
{
    internal class OxygenAirRefreshService : IOxygenAirRefreshService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly int _atpThreshold = 5;
        private readonly Air _air;

        public OxygenAirRefreshService(IRedisCacheService cacheService, Air air)
        {
            _cacheService = cacheService;
            _air = air;
        }

        public async Task PerformMotionAndRefreshAirAsync()
        {
            await PerformMotionAsync();
            RefreshAirInLungs();
        }

        private async Task PerformMotionAsync()
        {
            var organName = "lungs";
            await CacheHelper.PerformFunctionAsync(organName, _cacheService, async (Myocyte cell) =>
            {
                cell.PerformMotion(_atpThreshold);
                await Task.CompletedTask;
            });
        }

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}
