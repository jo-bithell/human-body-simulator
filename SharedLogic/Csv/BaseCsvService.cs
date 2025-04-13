using SharedLogic.Models.Cells;
using SharedLogic.Services;
using System.Text.Json;

namespace CsvCommon
{
    public abstract class BaseCsvService : IBaseCsvService
    {
        private readonly int _atpThreshold = 5;
        private readonly IRedisCacheService _cacheService;
        private readonly string _projectCalledFrom;
        public BaseCsvService(IRedisCacheService cacheService, string projectCalledFrom)
        {
            _cacheService = cacheService;
            _projectCalledFrom = projectCalledFrom;
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

        public virtual async Task DigestFood(List<string[]> records, string outputDirectory) => await Task.CompletedTask;

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
                var key = $"{_projectCalledFrom}-{nameof(Myocyte)}-{i.ToString()}";
                var cell = await _cacheService.GetAsync<Myocyte>(key);

                if (cell != null)
                    cell.PerformMotion(_atpThreshold);

                var serializedCell = JsonSerializer.Serialize(cell);
                await _cacheService.SetAsync(key, serializedCell);
            }
        }
    }
}
