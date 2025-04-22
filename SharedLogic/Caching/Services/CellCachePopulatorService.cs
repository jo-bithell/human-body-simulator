using Microsoft.Extensions.Hosting;
using SharedLogic.Caching.Services.Interfaces;
using System.Text.Json;

namespace SharedLogic.Caching.Services
{
    public class CellCachePopulatorService<Cell>: IHostedService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _organName;

        public CellCachePopulatorService(IRedisCacheService cacheService, string organName)
        {
            _cacheService = cacheService;
            _organName = organName;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serializedCell = JsonSerializer.Serialize(Activator.CreateInstance<Cell>());
            await _cacheService.SetAsync($"{_organName}-{typeof(Cell).Name.ToLower()}", serializedCell);

            Console.WriteLine($"Populated cache with {typeof(Cell).Name} cells.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
