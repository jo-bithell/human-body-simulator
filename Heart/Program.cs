using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using SharedLogic.Messaging;
using SharedLogic.Services;

namespace Heart
{
    class Program
    {
        private static int _numberOfCells = 100;
        static async Task Main(string[] args)
        {
            var deoxygenatedHostTask = CreateRightAtriumHostBuilder(args).Build().RunAsync();
            //var oxygenatedHostTask = CreateLeftAtriumHostBuilder(args).Build().RunAsync();

            await Task.WhenAll(deoxygenatedHostTask);
        }

        public static IHostBuilder CreateRightAtriumHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SnapshotCache<Blood>>();
                services.AddHostedService(provider =>
                {
                    var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
                    return new MessageConsumer<Blood>(snapshotCache, "right-atrium");
                });
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
                    return new MessagePublisher<Blood>("lungs");
                });
                services.AddScoped<HeartBloodProducerWorker>();
                services.AddHostedService<BloodCachePopulatorService>();
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
                services.AddQuartzHostedService();

            });

        //public static IHostBuilder CreateLeftAtriumHostBuilder(string[] args) =>
        //Host.CreateDefaultBuilder(args)
        //    .ConfigureServices((hostContext, services) =>
        //    {
        //        services.AddSingleton<SnapshotCache<Blood>>();
        //        services.AddSingleton<List<Myocyte>>();
        //        services.AddHostedService(provider =>
        //        {
        //            var snapshotCache = provider.GetRequiredService<SnapshotCache<Blood>>();
        //            return new MessageConsumer<Blood>(snapshotCache, "left-atrium");
        //        });
        //        services.AddSingleton(provider =>
        //        {
        //            var myocytes = new List<Myocyte>();
        //            for (int i = 0; i < _numberOfCells; i++)
        //            {
        //                myocytes.Add(new Myocyte());
        //            }
        //            return myocytes;
        //        });
        //        services.AddSingleton(provider =>
        //        {
        //            return new MessagePublisher<Blood>("rest-of-the-body");
        //        });
        //    });
    }
}