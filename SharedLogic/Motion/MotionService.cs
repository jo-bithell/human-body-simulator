using Microsoft.Extensions.Logging;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Motion.Services;

namespace SharedLogic.Motion
{
    public class MotionService : IMotionService
    {
        private readonly ICacheManagementService<Myocyte> _cacheManagementService;
        private readonly ILogger<MotionService> _logger;
        public MotionService(ICacheManagementService<Myocyte> cacheManagementService, ILogger<MotionService> logger)
        {
            _cacheManagementService = cacheManagementService;
            _logger = logger;
        }

        public async Task<bool> CanPerformMotionAsync(int atpThreshold)
        {
            return await _cacheManagementService.CellHasSufficientATP(atpThreshold);
        }

        public async Task PerformMotionAsync(int atpThreshold)
        {
            await _cacheManagementService.PerformFunctionAsync(async (cell) =>
            {
                PerformMotion(cell, atpThreshold);
                await Task.CompletedTask;
            });
        }

        private void PerformMotion(Myocyte myocyte, int atpThreshold)
        {
            if (myocyte.NutrientConcentrations.ATPCount < atpThreshold)
            {
                _logger.LogWarning("ATP count insufficient.");
                return;
            }

            myocyte.NutrientConcentrations.ATPCount = Math.Max(0, myocyte.NutrientConcentrations.ATPCount - atpThreshold);

            _logger.LogInformation($"Motion performed, {atpThreshold} ATP consumed");
        }
    }
}
