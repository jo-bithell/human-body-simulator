using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedLogic.Messaging.Models;
using SharedLogic.Models;

namespace SharedLogic.Caching.Services
{
    public class BloodCachePopulatorService: IHostedService
    {
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly ILogger<BloodCachePopulatorService> _logger;

        public BloodCachePopulatorService(SnapshotCache<Blood> bloodCache, ILogger<BloodCachePopulatorService> logger)
        {
            _bloodCache = bloodCache;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Blood newBlood = new Blood();
            _bloodCache.Queue.Enqueue(newBlood);

            _logger.LogInformation("Blood cache populated");
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
