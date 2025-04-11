using Microsoft.Extensions.Hosting;
using SharedLogic.Messaging;
using SharedLogic.Models;
using System.Text.Json;

namespace SharedLogic.Services
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

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
