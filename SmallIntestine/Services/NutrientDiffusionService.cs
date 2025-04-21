using SharedLogic.Models.Cells;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace SmallIntestine.Services
{
    internal class NutrientDiffusionService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;

        public NutrientDiffusionService(IRedisCacheService cacheService, string organName)
        {
            _cacheService = cacheService;
            _organName = organName;
        }

        internal async Task DiffuseGlucoseAsync(int glucoseCount)
        {
            await CacheHelper.PerformFunctionOnCellAsync(_organName, _cacheService, async (Enterocyte cell) =>
            {
                // perform function
                await Task.CompletedTask;
            });
        }
    }
}
