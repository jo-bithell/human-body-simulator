using SharedLogic.Diffusion.Services.Interfaces;
using SharedLogic.Messaging.Models;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Caching.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace SharedLogic.Diffusion.Services
{
    public class BloodDiffusionService<C> : IDiffusionService where C : Cell
    {
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly ICacheManagementService<C> _cacheManagementService;
        private readonly ILogger<BloodDiffusionService<C>> _logger;

        public BloodDiffusionService(SnapshotCache<Blood> bloodCache, ICacheManagementService<C> cacheManagementService, ILogger<BloodDiffusionService<C>> logger)
        {
            _bloodCache = bloodCache;
            _cacheManagementService = cacheManagementService;
            _logger = logger;
        }

        public async Task DiffuseAsync()
        {
            while (_bloodCache.Queue.TryDequeue(out var blood))
            {
                _logger.LogInformation("Diffusing nutrients");

                await DiffuseNutrientsFromBlood(blood);
                _bloodCache.Queue.Enqueue(blood);

                _logger.LogInformation("Nutrients diffused");
            }
        }

        private async Task DiffuseNutrientsFromBlood(Blood blood)
        {
            await _cacheManagementService.PerformFunctionAsync(async (C c) =>
            {
                PerformDiffusionBetweenBloodAndCell(blood, c);
                await Task.CompletedTask;
            });
        }

        private void PerformDiffusionBetweenBloodAndCell(Blood blood, C cell)
        {
            // Can't simulate diffusion completely due to blood having different local concentrations of nutrients
            DiffuseCarbonDioxide(blood, cell);
            DiffuseGlucose(blood, cell);
            DiffuseOxygen(blood, cell);
            DiffuseWater(blood, cell);
        }

        private void DiffuseWater(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.NutrientConcentrations.WaterCount, cell.NutrientConcentrations.WaterCount))
            {
                cell.NutrientConcentrations.WaterCount += 1;
                blood.WaterCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.NutrientConcentrations.WaterCount, blood.WaterCount))
            {
                cell.NutrientConcentrations.WaterCount -= 1;
                blood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.NutrientConcentrations.GlucoseCount, blood.GlucoseCount))
            {
                cell.NutrientConcentrations.GlucoseCount += 1;
                blood.GlucoseCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.NutrientConcentrations.GlucoseCount, blood.GlucoseCount))
            {
                cell.NutrientConcentrations.GlucoseCount -= 1;
                blood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.NutrientConcentrations.OxygenCount, blood.OxygenCount))
            {
                cell.NutrientConcentrations.OxygenCount += 1;
                blood.OxygenCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.NutrientConcentrations.OxygenCount, blood.OxygenCount))
            {
                cell.NutrientConcentrations.OxygenCount -= 1;
                blood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.NutrientConcentrations.CarbonDioxideCount, blood.CarbonDioxideCount))
            {
                cell.NutrientConcentrations.CarbonDioxideCount += 1;
                blood.CarbonDioxideCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.NutrientConcentrations.CarbonDioxideCount, blood.CarbonDioxideCount))
            {
                cell.NutrientConcentrations.CarbonDioxideCount -= 1;
                blood.CarbonDioxideCount += 1;
            }
        }
    }
}
