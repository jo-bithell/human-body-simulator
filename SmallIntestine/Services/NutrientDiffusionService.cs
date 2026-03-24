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
                DiffuseGlucose(cell, glucoseCount);
                await Task.CompletedTask;
            });
        }

        private void DiffuseGlucose(Enterocyte cell, int glucoseCount)
        {
            while (!cell.ConcentrationHigherInCell(cell.NutrientConcentrations.GlucoseCount, glucoseCount))
            {
                cell.NutrientConcentrations.GlucoseCount += 1;
            }

            while (cell.ConcentrationHigherInCell(cell.NutrientConcentrations.GlucoseCount, glucoseCount))
            {
                cell.NutrientConcentrations.GlucoseCount -= 1;
            }
        }
    }
}
