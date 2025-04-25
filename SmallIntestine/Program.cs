using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Digestion.Services.Interfaces;
using SmallIntestine.Services;
using SmallIntestine.Models;

namespace SmallIntestine
{
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
                ServiceRegistrationHelper.RegisterCommonServices("small-intestine", services);
                ServiceRegistrationHelper.RegisterServicesForCell<Enterocyte>(services);
                ServiceRegistrationHelper.RegisterDigestionServices(services);

                services.AddSingleton<NutrientDiffusionService>();
                services.AddSingleton<IDigestionService, SmallIntestineDigestionService>();
            });
    }
}