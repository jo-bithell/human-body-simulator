using CsvCommon;
using SharedLogic.Enums;
using SharedLogic.Models.Cells;
using SharedLogic.Services;
using System.Text.Json;

namespace SmallIntestine
{
    public class ChemicalDigestionService : BaseCsvService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _projectCalledFrom;
        public ChemicalDigestionService(IRedisCacheService cacheService, string projectCalledFrom)
            : base(cacheService, projectCalledFrom)
        {
            _cacheService = cacheService;
            _projectCalledFrom = projectCalledFrom;
        }

        public override async Task DigestFood(List<string[]> inputArray, string outputDirectory)
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
                var key = $"{_projectCalledFrom}-{nameof(Enterocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Enterocyte>(key);
                if (cell != null)
                    cell.DiffuseNutrients(glucoseCount);
                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}
