using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using Quartz;
using StackExchange.Redis;
using SharedLogic.Redis;
using SharedLogic.Diffusion;

namespace LeftAtrium
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var oxygenatedHostTask = CreateLeftAtriumHostBuilder(args).Build().RunAsync();

            await Task.WhenAll(oxygenatedHostTask);
        }

        public static IHostBuilder CreateLeftAtriumHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Core services
                services.AddSingleton("left-atrium");
                services.AddSingleton<SnapshotCache<Blood>>();

                // Redis
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
                services.AddSingleton<IRedisCacheService, RedisCacheService>();
                services.AddHostedService<CellCachePopulatorService<Myocyte>>();

                // RabbitMQ
                services.AddScoped<BloodProducerJob>();
                services.AddHostedService<MessageConsumer<Blood>>();

                //Quartz
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
                services.AddQuartzHostedService();
            });
    }
}