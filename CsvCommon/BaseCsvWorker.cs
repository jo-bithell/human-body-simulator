using Microsoft.Extensions.Hosting;

namespace CsvCommon
{
    public class BaseCsvWorker : IHostedService
    {
        private readonly BaseCsvService _csvService;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _backgroundTask;
        private string _inputDirectory;
        private string _outputDirectory;
        public BaseCsvWorker(BaseCsvService csvService, string inputDirectory, string outputDirectory)
        {
            _csvService = csvService;
            _inputDirectory = CreateDirectory(inputDirectory);
            _outputDirectory = CreateDirectory(outputDirectory);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _backgroundTask = Task.Run(() => PollQueue(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

            return Task.CompletedTask;
        }

        public virtual async Task PollQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var filePath in Directory.GetFiles(_inputDirectory, $"*.csv"))
                {
                    if (File.Exists(filePath))
                    {
                        var records = _csvService.ReadCsvFile(filePath);
                        _csvService.ProcessCsvFile(records, _outputDirectory);
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource?.Cancel();
            if (_backgroundTask != null)
            {
                return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

            return Task.CompletedTask;
        }

        public string CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}
