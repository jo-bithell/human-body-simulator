using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Redis;

namespace SharedLogic.Diffusion
{
    public class BloodDiffusionServiceFactory
    {
        private readonly string _organName;
        private readonly IRedisCacheService _cacheService;
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly Dictionary<string, Func<IDiffusionService>> _diffusionServiceStrategies;

        public BloodDiffusionServiceFactory(string organName, IRedisCacheService cacheService, SnapshotCache<Blood> bloodCache)
        {
            _organName = organName;
            _cacheService = cacheService;
            _bloodCache = bloodCache;

            // Define strategies for creating diffusion services
            _diffusionServiceStrategies = new Dictionary<string, Func<IDiffusionService>>
            {
                { "lungs", () => new BloodDiffusionService<AlveolarCell>(_bloodCache, _cacheService, _organName) },
                { "small-intestine", () => new BloodDiffusionService<Enterocyte>(_bloodCache, _cacheService, _organName) }
            };
        }

        public List<IDiffusionService> Create()
        {
            var diffusionServices = new List<IDiffusionService>
            {
                new BloodDiffusionService<Myocyte>(_bloodCache, _cacheService, _organName)
            };

            foreach (var strategy in _diffusionServiceStrategies)
            {
                if (_organName.Contains(strategy.Key))
                {
                    diffusionServices.Add(strategy.Value());
                }
            }

            return diffusionServices;
        }
    }
}
