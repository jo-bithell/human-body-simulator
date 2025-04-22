using SharedLogic.Caching.Services.Interfaces;
using SmallIntestine.Models;

namespace SmallIntestine.Services
{
    internal class NutrientDiffusionService
    {
        private readonly ICacheManagementService<Enterocyte> _cacheManagementService;

        public NutrientDiffusionService(ICacheManagementService<Enterocyte> cacheManagementService)
        {
            _cacheManagementService = cacheManagementService;
        }

        internal async Task DiffuseGlucoseAsync(int glucoseCount)
        {
            await _cacheManagementService.PerformFunctionAsync(async (Enterocyte cell) =>
            {
                // perform function
                await Task.CompletedTask;
            });
        }
    }
}
