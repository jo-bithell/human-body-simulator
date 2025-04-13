using CsvCommon;
using Quartz;

namespace SharedLogic
{
    public class CsvJob<C> : IJob where C : BaseCsvService
    {
        private readonly C _csvService;
        private string _inputDirectory;
        private string _outputDirectory;

        public CsvJob(C csvService, string inputDirectory, string outputDirectory)
        {
            _csvService = csvService;
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var filePath in Directory.GetFiles(_inputDirectory, $"*.csv"))
            {
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Received csv");
                    var records = _csvService.ReadCsvFile(filePath);
                    await _csvService.DigestFood(records, _outputDirectory);
                    Console.WriteLine("Sent csv");
                    //File.Delete(filePath); // Delete the file after reading
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }

            await Task.CompletedTask;
        }
    }
}