using Microsoft.Extensions.Hosting;
using SharedLogic.Messaging.Models;
using SharedLogic.Models;

namespace SharedLogic.Caching.Services
{
    public class BloodCachePopulatorService: IHostedService
    {
        private readonly SnapshotCache<Blood> _bloodCache;

        public BloodCachePopulatorService(SnapshotCache<Blood> bloodCache)
        {
            _bloodCache = bloodCache;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Blood newBlood = new Blood();
            _bloodCache.Queue.Enqueue(newBlood);

            Console.WriteLine("Blood cache populated");
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
