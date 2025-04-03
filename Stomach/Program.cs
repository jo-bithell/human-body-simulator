using SharedLogic.Models;
using KafkaCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Models.Cells;
using SharedLogic;
using Mouth;
using Stomach;

class Program
{
    private static int _numberOfCells = 100;
    private static int _digestionChunkSize = 10;
    private static string _inputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "input"));
    private static string _outputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "LargeIntestine", "input"));
    static async Task Main(string[] args)
    {
        await WorkerScheduler.ScheduleJobs();
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton(provider =>
            {
                var mycotes = new List<Myocyte>();
                for (int i = 0; i < _numberOfCells; i++)
                {
                    mycotes.Add(new Myocyte());
                }
                return mycotes;
            });
            services.AddSingleton(provider =>
            {
                var myocytes = provider.GetRequiredService<List<Myocyte>>();
                return new MechanicalDigestionService(myocytes, _digestionChunkSize);
            });
            services.AddSingleton<SnapshotCache<Blood>>();
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