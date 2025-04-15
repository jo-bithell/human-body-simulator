using Microsoft.Extensions.Hosting;
using SharedLogic.Messaging;
using SharedLogic.Models;

namespace SharedLogic.Redis
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
            for (int i = 0; i < 5; i++)
            {
                Blood newBlood = new Blood();
                _bloodCache.Queue.Enqueue(newBlood);
            }

            Console.WriteLine("Blood cache populated");
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
