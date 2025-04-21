using SharedLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Models.Cells;
using Quartz;
using SharedLogic;
using Lungs.Services;
using Lungs.Services.Interfaces;
using Lungs.Jobs;
using Lungs.Models;
class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                ServiceRegistrationHelper.RegisterCommonServices("lungs", services);
                ServiceRegistrationHelper.RegisterServicesForCell<AlveolarCell>(services);

                services.AddSingleton<Air>();
                services.AddScoped<IOxygenAirRefreshService, OxygenAirRefreshService>();
                services.AddScoped<OxygenAirRefreshJob>();
                services.AddQuartz(q =>
                {
                    q.ScheduleJob<OxygenAirRefreshJob>(trigger => trigger
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(5)
                            .RepeatForever()));
                });
            });
}
