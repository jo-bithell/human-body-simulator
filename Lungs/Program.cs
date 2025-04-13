using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lungs;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Services;
using StackExchange.Redis;
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
                services.AddSingleton("lungs");
                services.AddSingleton<SnapshotCache<Blood>>();
                services.AddSingleton<Air>();

                // Redis
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
                services.AddSingleton<IRedisCacheService, RedisCacheService>();
                services.AddHostedService<CellCachePopulatorService<Myocyte>>();
                services.AddHostedService<CellCachePopulatorService<AlveolarCell>>();

                // RabbitMQ
                services.AddHostedService<MessageConsumer<Blood>>();
                services.AddSingleton(provider => new MessagePublisher<Blood>("left-atrium"));

                // Quartz job scheduling
                services.AddQuartz(q =>
                {
                    q.ScheduleJob<BloodProducerJob>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));

                    q.ScheduleJob<BloodDiffusionJob<Myocyte>>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));

                    q.ScheduleJob<BloodDiffusionJob<AlveolarCell>>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));

                    q.ScheduleJob<BreathingJob>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));

                    q.ScheduleJob<AirDiffusionJob>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));
                });
            });
}
