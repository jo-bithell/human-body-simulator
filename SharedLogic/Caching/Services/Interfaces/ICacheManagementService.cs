using SharedLogic.Models.Cells;

namespace SharedLogic.Caching.Services.Interfaces
{
    public interface ICacheManagementService<C> where C : Cell
    {
        Task PerformFunctionAsync(Func<C, Task> function);
        Task<C?> GetCellFromCacheAsync();
        Task SetCellToCacheAsync(C cell);
    }
}