using CsvCommon;
using SharedLogic.Workers;

namespace SharedLogic
{
    public class CsvWorker<C> : BaseWorker where C : BaseCsvService
    {
        private readonly C _csvService;
        private string _inputDirectory;
        private string _outputDirectory;

        public CsvWorker(C csvService, string inputDirectory, string outputDirectory)
        {
            _csvService = csvService;
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
        }

        public override async Task PerformAction(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var filePath in Directory.GetFiles(_inputDirectory, $"*.csv"))
                {
                    if (File.Exists(filePath))
                    {
                        Console.WriteLine("Received csv");
                        var records = _csvService.ReadCsvFile(filePath);
                        _csvService.DigestFood(records, _outputDirectory);
                        Console.WriteLine("Sent csv");
                        //File.Delete(filePath); // Delete the file after reading
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {filePath}");
                    }
                }

                // Delay to avoid busy-waiting
                await Task.Delay(10000, cancellationToken); // Poll every 10 seconds
            }
        }
    }
}
