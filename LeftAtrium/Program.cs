using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using Quartz;

namespace Heart
{
    class Program
    {
        private static int _numberOfCells = 5;
        static async Task Main(string[] args)
        {
            var oxygenatedHostTask = CreateLeftAtriumHostBuilder(args).Build().RunAsync();

            await Task.WhenAll(oxygenatedHostTask);
        }

        public static IHostBuilder CreateLeftAtriumHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SnapshotCache<Blood>>();
                services.AddHostedService(provider =>
                {
                    var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
                    return new MessageConsumer<Blood>(snapshotCache, "left-atrium");
                });
                services.AddSingleton(provider =>
                {
                    var myocytes = new List<Myocyte>();
                    services.AddSingleton(provider =>
                    {
                        var myocytes = new List<Myocyte>();
                        for (int i = 0; i < _numberOfCells; i++)
                        {
                            myocytes.Add(new Myocyte());
                        }

                        return myocytes;
                    });
                    return myocytes;
                });
                services.AddScoped<HeartBloodProducerWorker>();
                services.AddScoped<BloodDiffusionWorker<Myocyte>>();
                services.AddQuartz(q =>
                {
                    q.ScheduleJob<HeartBloodProducerWorker>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));

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
            });
    }
}