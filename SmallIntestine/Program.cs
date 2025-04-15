using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic.Messaging;
using StackExchange.Redis;
using SharedLogic.Redis;
using SharedLogic.Diffusion;
using SharedLogic.Digestion;

namespace SmallIntestine
{
    class Program
    {
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
                // Core services
                services.AddSingleton("small-intestine");
                services.AddSingleton<SnapshotCache<Blood>>();
                services.AddSingleton<ChemicalDigestionService>();

                // Redis
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
                services.AddSingleton<IRedisCacheService, RedisCacheService>();
                services.AddHostedService<CellCachePopulatorService<Myocyte>>();
                services.AddHostedService<CellCachePopulatorService<Enterocyte>>();

                // RabbitMQ
                services.AddHostedService<MessageConsumer<Blood>>();
                services.AddSingleton<MessagePublisherFactory>();

                // Quartz
                services.AddQuartz(q =>
                {
                    q.ScheduleJob<BloodDiffusionJob<Myocyte>>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));

                    q.ScheduleJob<BloodDiffusionJob<Enterocyte>>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));

                    q.ScheduleJob<BloodProducerJob>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));

                    q.ScheduleJob<DigestionJob>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));
                });
                services.AddQuartzHostedService();
            });
    }
}