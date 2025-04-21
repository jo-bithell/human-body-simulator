using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic;
using SharedLogic.Caching.Services;

namespace RightAtrium
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var deoxygenatedHostTask = CreateRightAtriumHostBuilder(args).Build().RunAsync();
            await Task.WhenAll(deoxygenatedHostTask);
        }

        static IHostBuilder CreateRightAtriumHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                ServiceRegistrationHelper.RegisterCommonServices("right-atrium", services);
                services.AddHostedService<BloodCachePopulatorService>();
            });

    }
}