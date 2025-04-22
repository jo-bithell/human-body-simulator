using SharedLogic.Respiration.Services.Interfaces;
using SharedLogic.Respiration.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration.Factories
{
    public class RespirationServiceFactory<C> : IRespirationServiceFactory where C : Cell
    {
        private readonly ICacheManagementService<C> _cacheManagementService;
        public RespirationServiceFactory(ICacheManagementService<C> cacheManagementService)
        {
            _cacheManagementService = cacheManagementService;
        }
        public async Task<IRespirationService> GetServiceForRespiration()
        {
            var cell = await _cacheManagementService.GetCellFromCacheAsync();

            if (cell != null)
            {
                var respirationService = GetRespirationService(cell);
                return respirationService;
            }
            else
            {
                throw new InvalidOperationException($"Cell not found in cache.");
            }
        }

        private IRespirationService GetRespirationService(C cell)
        {
            if (cell.GlucoseCount > 0 && cell.OxygenCount > 0)
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new GlucoseRespirationService(cell);
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }
    }
}
