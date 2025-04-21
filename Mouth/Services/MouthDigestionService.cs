using SharedLogic.Digestion.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace Mouth.Services
{
    internal class MouthDigestionService : DigestionService
    {
        private readonly string OutputDirectory = GetOutputDirectory("Stomach");
        public MouthDigestionService(IRedisCacheService cacheService, string organName)
            : base(cacheService, organName)
        {
        }

        public override async Task DigestAsync()
        {
            EnsureDirectoriesExists(OutputDirectory);
            
            await DigestFoodAsync(async records =>
            {
                await PerformDigestionAsync(records);
            });
        }

        private async Task PerformDigestionAsync(IEnumerable<string[]> records)
        {
            await PerformMotionAsync();
            DivideFoodIntoChunks(records);
        }

        private void DivideFoodIntoChunks(IEnumerable<string[]> nutrientList)
        {
            int chunkSize = 10;
            int fileIndex = 0;

            var chunks = nutrientList
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / chunkSize)
                .Select(group => group.Select(x => x.value).ToList());

            foreach (var chunk in chunks)
            {
                string outputFilePath = Path.Combine(OutputDirectory, $"chunk_{fileIndex}.csv");
                WriteCsvFile(outputFilePath, chunk);
                fileIndex++;
            }
        }
    }
}
