using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SharedLogic;
using SharedLogic.Diffusion;
using SharedLogic.Digestion;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Redis;
using StackExchange.Redis;

namespace Mouth
{
    class Program
    {
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
            services.AddSingleton<DigestionServiceFactory>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddHostedService<CellCachePopulatorService<Myocyte>>();

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

                q.ScheduleJob<BloodProducerJob>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<DigestionJob> (trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
            services.AddQuartzHostedService();
        });
    }
}