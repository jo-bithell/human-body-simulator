using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Services;
using StackExchange.Redis;

class Program
{
    private static int _digestionChunkSize = 10;
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
            services.AddSingleton("stomach");
            services.AddSingleton<SnapshotCache<Blood>>();
            services.AddSingleton<MechanicalDigestionService>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddHostedService<CellCachePopulatorService<Myocyte>>();

            // RabbitMQ
            services.AddHostedService<MessageConsumer<Blood>>();
            services.AddSingleton(provider =>
            {
                return new MessagePublisher<Blood>("right-atrium");
            });

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
            });
        });
}