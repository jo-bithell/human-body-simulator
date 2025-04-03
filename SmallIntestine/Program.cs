using SharedLogic.Models;
using KafkaCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Models.Cells;

namespace SmallIntestine
{
    class Program
    {
        private static int _numberOfCells = 100;
        private static string _inputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "input"));
        private static string _outputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "LargeIntestine", "input"));
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(provider =>
                {
                    var mouthCells = new List<Myocyte>();
                    for (int i = 0; i < _numberOfCells; i++)
                    {
                        mouthCells.Add(new Myocyte());
                    }
                    return mouthCells;
                });
                services.AddSingleton(provider =>
                {
                    var mouthCells = new List<Enterocyte>();
                    for (int i = 0; i < _numberOfCells; i++)
                    {
                        mouthCells.Add(new Enterocyte());
                    }
                    return mouthCells;
                });
                services.AddSingleton<SnapshotCache<Blood>>();
                services.AddSingleton<ChemicalDigestionService>();
                services.AddHostedService(provider =>
                {
                    var csvService = provider.GetRequiredService<ChemicalDigestionService>();
                    return new CsvWorker<ChemicalDigestionService>(csvService, _inputDirectory, _outputDirectory);
                });
                services.AddHostedService(provider =>
                {
                    var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
                    return new MessageConsumer<Blood>(snapshotCache, "rest-of-the-body");
                });
                services.AddSingleton(provider =>
                {
                    return new MessagePublisher<Blood>("right-atrium");
                });
            });
    }
}