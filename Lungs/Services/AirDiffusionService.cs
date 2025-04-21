using SharedLogic.Models.Cells;
using SharedLogic.Diffusion.Services.Interfaces;
using Lungs.Models;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace Lungs.Services
{
    internal class AirDiffusionService : DiffusionService
    {
        private readonly string _organName;
        private readonly IRedisCacheService _cacheService;
        private Air _air;
        internal AirDiffusionService(string organName, IRedisCacheService cacheService, Air air)
        {
            _air = air;
            _organName = organName;
            _cacheService = cacheService;
        }

        public async Task DiffuseAsync()
        {
            await CacheHelper.PerformFunctionOnCellAsync(_organName, _cacheService, async (AlveolarCell cell) =>
            {
                DiffuseFromAir(cell);
                await Task.CompletedTask;
            });
        }

        private void DiffuseFromAir(AlveolarCell cell)
        {
            Console.WriteLine("Diffusing nutrients");

            DiffuseCarbonDioxide(cell);
            DiffuseOxygen(cell);

            Console.WriteLine("Nutrients diffused");
        }

        private void DiffuseCarbonDioxide(AlveolarCell cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.CarbonDioxideCount, _air.CarbonDioxideCount))
            {
                cell.CarbonDioxideCount += 1;
                _air.CarbonDioxideCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.CarbonDioxideCount, _air.CarbonDioxideCount))
            {
                cell.CarbonDioxideCount -= 1;
                _air.CarbonDioxideCount += 1;
            }
        }

        private void DiffuseOxygen(AlveolarCell cell)
        {
            while (!cell.ConcentrationHigherInCell(cell.OxygenCount, _air.OxygenCount))
            {
                cell.OxygenCount += 1;
                _air.OxygenCount -= 1;
            }

            while (cell.ConcentrationHigherInCell(cell.OxygenCount, _air.OxygenCount))
            {
                cell.OxygenCount -= 1;
                _air.OxygenCount += 1;
            }
        }
    }
}
