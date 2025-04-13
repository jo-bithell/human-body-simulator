using CsvCommon;

namespace SharedLogic.Services
{
    public class MechanicalDigestionService : BaseCsvService
    {
        private readonly int _atpThreshold = 5;
        private readonly int _chunkSize = 10;
        private readonly IRedisCacheService _cacheService;
        private readonly string _projectCalledFrom;
        public MechanicalDigestionService(IRedisCacheService cacheService, string projectCalledFrom)
            : base(cacheService, projectCalledFrom)
        {
            _cacheService = cacheService;
            _projectCalledFrom = projectCalledFrom;
        }

        public override async Task DigestFood(List<string[]> records, string outputDirectory)
        {
            await PerformRespiration();
            int fileIndex = 0;

            for (int i = 0; i < records.Count; i += _chunkSize)
            {
                var chunk = records.GetRange(i, Math.Min(_chunkSize, records.Count - i));
                string outputFilePath = Path.Combine(outputDirectory, $"chunk_{fileIndex}.csv");
                WriteCsvFile(outputFilePath, chunk);
                fileIndex++;
            }
        }
    }
}
