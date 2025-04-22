using SharedLogic.Models.Cells;
using SharedLogic.Diffusion.Services.Interfaces;
using Lungs.Models;
using SharedLogic.Caching.Services.Interfaces;

namespace Lungs.Services
{
    internal class AirDiffusionService : IDiffusionService
    {
        private readonly ICacheManagementService<AlveolarCell> _cacheManagementService;
        private Air _air;
        internal AirDiffusionService(ICacheManagementService<AlveolarCell> cacheManagementService, Air air)
        {
            _air = air;
            _cacheManagementService = cacheManagementService;
        }

        public async Task DiffuseAsync()
        {
            await _cacheManagementService.PerformFunctionAsync(async (AlveolarCell cell) =>
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
