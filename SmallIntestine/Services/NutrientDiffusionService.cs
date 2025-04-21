using SharedLogic.Models.Cells;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;
using SmallIntestine.Models;

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
            await CacheHelper.PerformFunctionAsync(_organName, _cacheService, async (Enterocyte cell) =>
            {
                // perform function
                await Task.CompletedTask;
            });
        }
    }
}
