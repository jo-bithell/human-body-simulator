using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace Mouth
{
    class Program
    {
        private static int _numberOfCells = 100;
        private static int _digestionChunkSize = 100;
        private static string _inputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "input"));
        private static string _outputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Stomach", "input"));
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
        {
            // Core services
            // RabbitMQ
            // Redis
            // Digestion
            // Quartz
            services.AddQuartz(q =>
            {
                q.ScheduleJob<BloodDiffusionWorker<Myocyte>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<BloodProducerWorker>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
            services.AddSingleton(provider =>
            {
                var myocytes = provider.GetRequiredService<List<Myocyte>>();
                return new MechanicalDigestionService(myocytes, _digestionChunkSize);
            });
            services.AddHostedService(provider =>
            {
                var csvService = provider.GetRequiredService<MechanicalDigestionService>();
                return new CsvWorker<MechanicalDigestionService>(
                    csvService,
                    _inputDirectory,
                    _outputDirectory);
            });
            services.AddSingleton<SnapshotCache<Blood>>();
            services.AddHostedService(provider =>
            {
                var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
                return new MessageConsumer<Blood>(snapshotCache, "mouth");
            });
            services.AddSingleton(provider => new MessagePublisher<Blood>("right-atrium"));
        });
    }
}