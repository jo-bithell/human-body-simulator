using SharedLogic.Redis;

namespace SharedLogic.Digestion
{
    public class MechanicalDigestionService : DigestionService
    {
        public MechanicalDigestionService(IRedisCacheService cacheService, string organName, string outputDirectory)
            : base(cacheService, organName, outputDirectory)
        {

        }

        public override async Task DigestFood(List<string[]> inputFood)
        {
            await PerformRespiration();
            DivideFoodIntoChunks(inputFood);
        }

        private void DivideFoodIntoChunks(List<string[]> nutrientList)
        {
            int totalNutrientChunks = 10;
            int chunkSize = (int)Math.Ceiling((double)nutrientList.Count / totalNutrientChunks);
            int fileIndex = 0;

            for (int i = 0; i < nutrientList.Count; i += chunkSize)
            {
                var chunk = nutrientList.GetRange(i, Math.Min(chunkSize, nutrientList.Count - i));
                string outputFilePath = Path.Combine(OutputDirectory, $"chunk_{fileIndex}.csv");
                WriteCsvFile(outputFilePath, chunk);
                fileIndex++;
            }
        }
    }
}
