using Quartz;

namespace SharedLogic.Digestion
{
    public class DigestionJob : IJob
    {
        private readonly DigestionService _csvService;
        private readonly string _currentFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "input"));

        public DigestionJob(DigestionServiceFactory digestionServiceFactory)
        {
            _csvService = digestionServiceFactory.Create();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var filePath in Directory.GetFiles(_currentFilePath, $"*.csv"))
            {
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Received csv");
                    var records = _csvService.ReadCsvFile(filePath);
                    await _csvService.DigestFood(records);
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