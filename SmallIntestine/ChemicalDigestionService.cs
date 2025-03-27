using CsvCommon;
using SharedLogic.Enums;
using SharedLogic.Models.Cells;

namespace SmallIntestine
{
    public class ChemicalDigestionService : BaseCsvService
    {
        private readonly int _atpThreshold = 5;
        private readonly List<Myocyte> _myocytes;
        private readonly List<Enterocyte> _enterocytes;
        public ChemicalDigestionService(List<Myocyte> mouthCells, List<Enterocyte> enterocytes)
        {
            _myocytes = mouthCells;
            _enterocytes = enterocytes;
        }

        public override void DigestFood(List<string[]> inputArray, string outputDirectory)
        {
            if (inputArray.Count == 0)
                return;

            PerformRespiration();
            ConvertCsvsToFood(inputArray);
        }

        private void ConvertCsvsToFood(List<string[]> inputArray)
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

            foreach (var enterocyte in _enterocytes)
            {
                enterocyte.DiffuseNutrients(glucoseCount);
            }
        }

        private void WriteCsvFile(string filePath, List<string> records)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var record in records)
                {
                    writer.WriteLine(string.Join(",", record));
                }
            }
        }

        protected override void PerformRespiration()
        {
            foreach (Myocyte myocyte in _myocytes)
            {
                myocyte.PerformMotion(_atpThreshold);
            }
        }
    }
}
