using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Redis;
using System.Text.Json;

namespace SharedLogic.Digestion
{
    public class ChemicalDigestionService : DigestionService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;
        public ChemicalDigestionService(IRedisCacheService cacheService, string organName, string outputDirectory)
            : base(cacheService, organName, outputDirectory)
        {
            _cacheService = cacheService;
            _organName = organName;
        }

        public override async Task DigestFood(List<string[]> inputArray)
        {
            if (inputArray.Count == 0)
                return;

            await PerformRespiration();
            await ConvertCsvsToFood(inputArray);
        }

        private async Task ConvertCsvsToFood(List<string[]> inputArray)
        {
            int glucoseCount = 0;
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

                if (nutrientType == NutrientType.Glucose)
                    glucoseCount += 1;
            }

            await DiffuseNutrients(glucoseCount);
        }

        private async Task DiffuseNutrients(int glucoseCount)
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"{_organName}-{nameof(Enterocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Enterocyte>(key);
                //if (cell != null)
                //    cell.DiffuseNutrientsFromBlood(glucoseCount);
                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}
