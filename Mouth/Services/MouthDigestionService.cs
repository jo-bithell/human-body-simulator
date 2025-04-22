using SharedLogic.Digestion.Services;
using SharedLogic.Motion.Services;

namespace Mouth.Services
{
    internal class MouthDigestionService : DigestionService
    {
        private readonly string OutputDirectory = GetOutputDirectory("Stomach");
        private readonly int _atpThreshold = 5;
        public MouthDigestionService(IMotionService motionService)
            : base(motionService)
        {
        }

        public override async Task DigestAsync()
        {
            EnsureDirectoriesExists(OutputDirectory);
            
            await DigestFoodAsync(async records =>
            {
               await PerformDigestion(records);
            });
        }

        private async Task PerformDigestion(IEnumerable<string[]> records)
        {
            await PerformMotion(_atpThreshold);
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
