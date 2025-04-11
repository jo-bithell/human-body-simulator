using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace SharedLogic.Services
{
    public class CellCachePopulatorService<Cell>: IHostedService
    {
        private readonly IRedisCacheService _cacheService;
        private readonly string _projectCalledFrom;

        public CellCachePopulatorService(IRedisCacheService cacheService, string projectCalledFrom)
        {
            _cacheService = cacheService;
            _projectCalledFrom = projectCalledFrom;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                var serializedCell = JsonSerializer.Serialize(Activator.CreateInstance<Cell>());
                await _cacheService.SetAsync($"{_projectCalledFrom}-{typeof(Cell).Name.ToLower()}-{i.ToString()}", serializedCell);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
