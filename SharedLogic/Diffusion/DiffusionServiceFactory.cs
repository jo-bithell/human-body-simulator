using SharedLogic.Models.Cells;
using SharedLogic.Redis;

namespace SharedLogic.Diffusion
{
    public class DiffusionServiceFactory
    {
        private readonly string _organName;
        private readonly IRedisCacheService _cacheService;

        public DiffusionServiceFactory(string organName, IRedisCacheService cacheService)
        {
            _organName = organName;
            _cacheService = cacheService;
        }

        public static IDiffusionService Create(string organName, IRedisCacheService cacheService)
        {
        }
    }
}
