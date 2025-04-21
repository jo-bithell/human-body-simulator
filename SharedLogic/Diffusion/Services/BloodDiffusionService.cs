using SharedLogic.Diffusion.Services.Interfaces;
using SharedLogic.Messaging.Models;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace SharedLogic.Diffusion.Services
{
    public class BloodDiffusionService<C> : DiffusionService where C : Cell
    {
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;
        public BloodDiffusionService(SnapshotCache<Blood> bloodCache, IRedisCacheService cacheService, string organName)
        {
            _bloodCache = bloodCache;
            _cacheService = cacheService;
            _organName = organName;
        }

        public async Task DiffuseAsync()
        {
            while (_bloodCache.Queue.TryDequeue(out var blood))
            {
                Console.WriteLine("Diffusing nutrients");

                await DiffuseNutrientsFromBlood(blood);
                _bloodCache.Queue.Enqueue(blood);

                Console.WriteLine("Nutrients diffused");
            }
        }

        private async Task DiffuseNutrientsFromBlood(Blood blood)
        {
            await CacheHelper.PerformFunctionOnCellAsync(_organName, _cacheService, async (C c) =>
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
            while (!cell.ConcentrationHigherInCell(cell.WaterCount, cell.WaterCount))
            {
                cell.WaterCount += 1;
                blood.WaterCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.WaterCount, blood.WaterCount))
            {
                cell.WaterCount -= 1;
                blood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.GlucoseCount, blood.GlucoseCount))
            {
                cell.GlucoseCount += 1;
                blood.GlucoseCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.GlucoseCount, blood.GlucoseCount))
            {
                cell.GlucoseCount -= 1;
                blood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.OxygenCount, blood.OxygenCount))
            {
                cell.OxygenCount += 1;
                blood.OxygenCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.OxygenCount, blood.OxygenCount))
            {
                cell.OxygenCount -= 1;
                blood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Blood blood, C cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.CarbonDioxideCount, blood.CarbonDioxideCount))
            {
                cell.CarbonDioxideCount += 1;
                blood.CarbonDioxideCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.CarbonDioxideCount, blood.CarbonDioxideCount))
            {
                cell.CarbonDioxideCount -= 1;
                blood.CarbonDioxideCount += 1;
            }
        }
    }
}
