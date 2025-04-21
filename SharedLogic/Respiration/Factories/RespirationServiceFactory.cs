using SharedLogic.Respiration.Services.Interfaces;
using SharedLogic.Respiration.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Caching.Services;
using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration.Factories
{
    public class RespirationServiceFactory<C> : IRespirationServiceFactory where C : Cell
    {
        private IRedisCacheService _cacheService;
        private readonly string _organName;
        public RespirationServiceFactory(IRedisCacheService cacheService, string organName)
        {
            _cacheService = cacheService;
            _organName = organName;
        }
        public async Task<IRespirationService> GetServiceForRespiration()
        {
            var cell = await CacheHelper.GetCellFromCacheAsync<C>(_organName, _cacheService);

            if (cell != null)
            {
                var respirationService = GetRespirationService(cell);
                return respirationService;
            }
            else
            {
                throw new InvalidOperationException($"Cell not found in cache for organ: {_organName}");
            }
        }

        private IRespirationService GetRespirationService(C cell)
        {
            if (cell.GlucoseCount > 0 && cell.OxygenCount > 0)
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new AerobicGlucoseMetabolism(cell);
            }

            if (cell.FattyAcidsCount > 0)
            {
                return new FattyAcidMetaboliser();
            }

            if (cell.AminoAcidsCount > 0)
            {
                return new AminoAcidMetaboliser();
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }
    }
}
