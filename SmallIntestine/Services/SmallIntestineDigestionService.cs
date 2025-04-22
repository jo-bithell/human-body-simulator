using SharedLogic.Digestion.Services;
using SharedLogic.Models.Enums;
using SharedLogic.Motion.Services;

namespace SmallIntestine.Services
{
    internal class SmallIntestineDigestionService : DigestionService
    {
        private readonly NutrientDiffusionService _nutrientDiffusionService;
        private readonly int _atpThreshold = 5;
        private readonly string OutputDirectory = GetOutputDirectory("LargeIntestine");
        public SmallIntestineDigestionService(IMotionService motionService, NutrientDiffusionService nutrientDiffusionService)
            : base(motionService)
        {
            _nutrientDiffusionService = nutrientDiffusionService;
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
            await PerformMotion(_atpThreshold);
            await ConvertCsvsToFoodAsync(records);
        }

        private async Task ConvertCsvsToFoodAsync(IEnumerable<string[]> inputArray)
        {
            var nutrientCounts = new Dictionary<NutrientType, int>
            {
                { NutrientType.Glucose, 0 },
                { NutrientType.Protein, 0 },
                { NutrientType.Fat, 0 }
            };

            foreach (string[] nutrientArray in inputArray)
            {
                var nutrient = nutrientArray[0];
                NutrientType nutrientType = nutrient switch
                {
                    "Carbohydrate" => NutrientType.Glucose,
                    "Protein" => NutrientType.Protein,
                    "Fat" => NutrientType.Fat,
                    _ => throw new NotImplementedException("Unrecognized nutrient type in csv.")
                };

                nutrientCounts[nutrientType]++;
            }

            await _nutrientDiffusionService.DiffuseGlucoseAsync(nutrientCounts[NutrientType.Glucose]);
            // Fats and proteins require further digestion to be absorbed.
        }
    }
}
