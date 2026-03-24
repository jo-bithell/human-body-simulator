using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Digestion.Services.Interfaces;
using Stomach.Services;

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
            ServiceRegistrationHelper.RegisterCommonServices("stomach", services);
            ServiceRegistrationHelper.RegisterDigestionServices(services);
            services.AddSingleton<IDigestionService, StomachDigestionService>();
        });
}