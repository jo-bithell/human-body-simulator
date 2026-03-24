using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedLogic.Caching.Services.Interfaces;
using System.Text.Json;

namespace SharedLogic.Caching.Services
{
    public class CellCachePopulatorService<Cell>: IHostedService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;
        private readonly ILogger<CellCachePopulatorService<Cell>> _logger;

        public CellCachePopulatorService(IRedisCacheService cacheService, string organName, ILogger<CellCachePopulatorService<Cell>> logger)
        {
            _cacheService = cacheService;
            _organName = organName;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serializedCell = JsonSerializer.Serialize(Activator.CreateInstance<Cell>());
            await _cacheService.SetAsync($"{_organName}-{typeof(Cell).Name.ToLower()}", serializedCell);

            _logger.LogInformation($"Populated cache with {typeof(Cell).Name} cells.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
