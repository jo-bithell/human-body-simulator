using SharedLogic.Redis;

namespace SharedLogic.Digestion
{
    public class DigestionServiceFactory
    {
        private readonly string _organName;
        private readonly IRedisCacheService _cacheService;
        private static readonly Dictionary<string, string> _organFileRoutingMap = new()
        {
            { "mouth", GetOutputDirectory("Stomach") },
            { "stomach", GetOutputDirectory("SmallIntestine") },
            { "small-intestine", GetOutputDirectory("LargeIntestine") },
        };
        public DigestionServiceFactory(string organName, IRedisCacheService cacheService)
        {
            _organName = organName;
            _cacheService = cacheService;
        }

        public DigestionService Create()
        {
            if (_organFileRoutingMap.TryGetValue(_organName, out var outputDirectory))
            {
                if (_organName == "mouth" || _organName == "stomach")
                {
                    return new MechanicalDigestionService(_cacheService, _organName, outputDirectory);
                }

                if (_organName == "small-intestine")
                {
                    return new ChemicalDigestionService(_cacheService, _organName, outputDirectory);
                }
            }

            throw new NotImplementedException();
        }

        private static string GetOutputDirectory(string organ)
            => Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", organ, "input"));
    }
}
