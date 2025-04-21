using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mouth.Services;
using SharedLogic;
using SharedLogic.Digestion.Services.Interfaces;

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
            ServiceRegistrationHelper.RegisterCommonServices("mouth", services);
            ServiceRegistrationHelper.RegisterDigestionServices(services);

            services.AddSingleton<IDigestionService, MouthDigestionService>();
        });
    }
}