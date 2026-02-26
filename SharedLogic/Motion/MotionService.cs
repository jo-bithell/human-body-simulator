using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Motion.Services;

namespace SharedLogic.Motion
{
    public class MotionService : IMotionService
    {
        private readonly ICacheManagementService<Myocyte> _cacheManagementService;
        public MotionService(ICacheManagementService<Myocyte> cacheManagementService)
        {
            _cacheManagementService = cacheManagementService;
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
                Console.WriteLine("ATP count insufficient.");
                return;
            }

            myocyte.NutrientConcentrations.ATPCount = Math.Max(0, myocyte.NutrientConcentrations.ATPCount - atpThreshold);

            Console.WriteLine($"Motion performed, {atpThreshold} ATP consumed");
        }
    }
}
