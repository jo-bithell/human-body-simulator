using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Services;
using StackExchange.Redis;

namespace Mouth
{
    class Program
    {
        // move to appsettings
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
            services.AddSingleton("mouth");
            services.AddSingleton<SnapshotCache<Blood>>();
            services.AddSingleton<MechanicalDigestionService>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddHostedService<CellCachePopulatorService<Myocyte>>();

            // RabbitMQ
            services.AddHostedService<MessageConsumer<Blood>>();
            services.AddSingleton(provider => new MessagePublisher<Blood>("right-atrium"));

            // Quartz
            services.AddQuartz(q =>
            {
                q.ScheduleJob<BloodDiffusionJob<Myocyte>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<BloodProducerJob>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob< CsvJob<MechanicalDigestionService>> (trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
            services.AddQuartzHostedService();
        });
    }
}