using Microsoft.Extensions.Logging;
using SharedLogic.Digestion.Services;
using SharedLogic.Motion.Services;

namespace Stomach.Services
{
    internal class StomachDigestionService : DigestionService
    {
        private string OutputDirectory = GetOutputDirectory("SmallIntestine");
        private int _atpThreshold = 5;
        private readonly ILogger<StomachDigestionService> _logger;
        public StomachDigestionService(IMotionService motionService, ILogger<StomachDigestionService> logger)
            : base(motionService, logger)
        {
            _logger = logger;
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
            if (!await CanPerformDigestion(_atpThreshold))
            {
                _logger.LogInformation("Insufficient ATP to perform digestion");
                return;
            }

            await PerformMotion(_atpThreshold);
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
