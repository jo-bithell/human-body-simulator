using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using StackExchange.Redis;
using SharedLogic.Redis;
using SharedLogic.Diffusion;

namespace RightAtrium
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var deoxygenatedHostTask = CreateRightAtriumHostBuilder(args).Build().RunAsync();
            await Task.WhenAll(deoxygenatedHostTask);
        }

        public static IHostBuilder CreateRightAtriumHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Core services
                services.AddSingleton("right-atrium");
                services.AddSingleton<SnapshotCache<Blood>>();

                // Redis
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
                services.AddSingleton<IRedisCacheService, RedisCacheService>();
                services.AddHostedService<CellCachePopulatorService<Myocyte>>();
                services.AddHostedService<BloodCachePopulatorService>();

                // RabbitMQ
                services.AddHostedService<MessageConsumer<Blood>>();
                services.AddSingleton<MessagePublisherFactory>();

                // Quartz
                services.AddQuartz(q =>
                {
                    q.ScheduleJob<BloodProducerJob>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));

                    q.ScheduleJob<DiffusionJob<Myocyte>>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));
                });
            });
    }
}