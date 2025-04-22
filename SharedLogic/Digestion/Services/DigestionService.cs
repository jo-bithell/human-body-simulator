using SharedLogic.Digestion.Services.Interfaces;
using SharedLogic.Motion.Services;

namespace SharedLogic.Digestion.Services
{
    public abstract class DigestionService : IDigestionService
    {
        public readonly static string InputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "input"));
        private readonly IMotionService _motionService;
        public DigestionService(IMotionService motionService)
        {
            _motionService = motionService;
        }

        public abstract Task DigestAsync();

        public IEnumerable<string[]> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');
                    if (values != null)
                    {
                        yield return values;
                    }
                }
        }

        public async Task DigestFoodAsync(Func<IEnumerable<string[]>, Task> func)
        {
            foreach (var filePath in Directory.GetFiles(InputDirectory, $"*.csv"))
            {
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Received csv");
                    var records = ReadCsvFile(filePath);

                    await func(records);
                    Console.WriteLine("Sent csv");
                    //File.Delete(filePath); // Delete the file after reading
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
        }

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

        public virtual async Task PerformMotion(int atpThreshold)
        {
            await _motionService.PerformMotionAsync(atpThreshold);
        }

        public static string GetOutputDirectory(string organ)
            => Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", organ, "input"));

        public static void EnsureDirectoriesExists(string outputDirectory)
        {
            if (!Directory.Exists(InputDirectory))
            {
                Directory.CreateDirectory(InputDirectory);
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
        }
    }
}
