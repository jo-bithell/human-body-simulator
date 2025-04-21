using SharedLogic.Digestion.Services;
using SharedLogic.Caching.Services.Interfaces;

namespace Stomach.Services
{
    internal class StomachDigestionService : DigestionService
    {
        private string OutputDirectory = GetOutputDirectory("SmallIntestine");
        public StomachDigestionService(IRedisCacheService cacheService, string organName)
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
            await PerformMotion();
            DivideFoodIntoChunks(records);
        }

        private void DivideFoodIntoChunks(IEnumerable<string[]> nutrientList)
        {
            int chunkSize = 3;
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
