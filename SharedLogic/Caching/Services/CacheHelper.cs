using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using System.Text.Json;

namespace SharedLogic.Caching.Services
{
    public static class CacheHelper
    {
        public static async Task PerformFunctionOnCellAsync<C>(string organName, IRedisCacheService cacheService, Func<C, Task> function) where C : Cell
        {
            var cell = await GetCellFromCacheAsync<C>(organName, cacheService);

            if (cell != null)
            {
                await function(cell);
                await SetCellToCacheAsync(organName, cell, cacheService);
            }
            else
            {
                throw new InvalidOperationException($"Cell not found in cache for organ: {organName}");
            }
        }

        public static async Task SetCellToCacheAsync<C>(string organName, C cell, IRedisCacheService cacheService, TimeSpan? expiry = null) where C : Cell
        {
            var key = GetCacheKey(organName, typeof(C).Name.ToLower());
            var cellJson = JsonSerializer.Serialize(cell);
            await cacheService.SetAsync(key, cellJson, expiry);
        }

        private static async Task<C?> GetCellFromCacheAsync<C>(string organName, IRedisCacheService cacheService) where C : Cell
        {
            var key = GetCacheKey(organName, typeof(C).Name.ToLower());
            var cellJson = await cacheService.GetAsync<string>(key);
            return cellJson != null ? JsonSerializer.Deserialize<C>(cellJson) : null;
        }

        private static string GetCacheKey(string organName, string cell)
            => $"{organName}-{cell}";
    }
}
