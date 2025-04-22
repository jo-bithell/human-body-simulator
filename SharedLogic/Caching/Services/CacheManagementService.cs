using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using System.Text.Json;

namespace SharedLogic.Caching.Services
{
    public class CacheManagementService<C> : ICacheManagementService<C> where C : Cell
    {
        private readonly string _organName;
        private readonly IRedisCacheService _cacheService;
        public CacheManagementService(string organName, IRedisCacheService cacheService)
        {
            _organName = organName;
            _cacheService = cacheService;
        }

        public async Task PerformFunctionAsync(Func<C, Task> function)
        {
            var cell = await GetCellFromCacheAsync();

            if (cell != null)
            {
                await function(cell);
                await SetCellToCacheAsync(cell);
            }
            else
            {
                throw new InvalidOperationException($"Cell not found in cache for organ: {_organName}");
            }
        }

        public async Task<C?> GetCellFromCacheAsync()
        {
            var key = GetCacheKey(_organName, typeof(C).Name.ToLower());
            var cellJson = await _cacheService.GetAsync<string>(key);
            return cellJson != null ? JsonSerializer.Deserialize<C>(cellJson) : null;
        }

        public async Task SetCellToCacheAsync(C cell)
        {
            var key = GetCacheKey(_organName, typeof(C).Name.ToLower());
            var cellJson = JsonSerializer.Serialize(cell);
            await _cacheService.SetAsync(key, cellJson);
        }

        private string GetCacheKey(string organName, string cell)
            => $"{organName}-{cell}";
    }
}
