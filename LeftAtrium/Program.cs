using Microsoft.Extensions.Hosting;
using SharedLogic;

namespace LeftAtrium
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var task = CreateHostBuilder(args).Build().RunAsync();
            await Task.WhenAll(task);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                ServiceRegistrationHelper.RegisterCommonServices("left-atrium", services);
            });
    }
}