using SharedLogic.Models.Cells;
using SharedLogic.Redis;
using System.Text.Json;

namespace SharedLogic.Digestion
{
    public abstract class DigestionService
    {
        public string OutputDirectory { get; set; }
        private readonly int _atpThreshold = 5;
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;
        public DigestionService(IRedisCacheService cacheService, string organName, string outputDirectory)
        {
            _cacheService = cacheService;
            _organName = organName;
            OutputDirectory = outputDirectory;
        }

        public List<string[]> ReadCsvFile(string filePath)
        {
            var records = new List<string[]>();

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');
                    if (values != null)
                    {
                        records.Add(values);
                    }
                }
            }

            return records;
        }

        public abstract Task DigestFood(List<string[]> records);

        public void WriteCsvFile(string filePath, List<string[]> records)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var record in records)
                {
                    writer.WriteLine(string.Join(",", record));
                }
            }
        }

        protected virtual async Task PerformRespiration()
        {
            for (int i = 0; i < 5; i++)
            {
                var key = $"{_organName}-{nameof(Myocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Myocyte>(key);

                if (cell != null)
                    cell.PerformMotion(_atpThreshold);

                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}
