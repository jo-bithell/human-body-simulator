using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lungs;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;

class Program
{
    private static int _numberOfCells = 100;
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<SnapshotCache<Blood>>();
            services.AddSingleton(provider =>
            {
                var myocytes = new List<Myocyte>();
                for (int i = 0; i < _numberOfCells; i++)
                {
                    myocytes.Add(new Myocyte());
                }
                return myocytes;
            });
            services.AddSingleton(provider =>
            {
                var alveolarCells = new List<AlveolarCell>();
                for (int i = 0; i < _numberOfCells; i++)
                {
                    alveolarCells.Add(new AlveolarCell());
                }
                return alveolarCells;
            });
            services.AddSingleton(provider =>
            {
                return new MessagePublisher<Blood>("left-atrium");
            });
            services.AddHostedService(provider =>
            {
                var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
                return new MessageConsumer<Blood>(snapshotCache, "lungs");
            });
            services.AddSingleton<Air>();
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

                q.ScheduleJob<BreathingWorker>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<AirDiffusionWorker>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<BloodDiffusionWorker<AlveolarCell>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });

        });
}
