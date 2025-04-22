using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Motion.Services;
using SharedLogic.Respiration.Factories;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Motion
{
    public class MotionService : IMotionService
    {
        private readonly IRespirationService _respirationService;
        private readonly ICacheManagementService<Myocyte> _cacheManagementService;
        public MotionService(IRespirationServiceFactory respirationServiceFactory, ICacheManagementService<Myocyte> cacheManagementService)
        {
            _respirationService = respirationServiceFactory.GetServiceForRespiration().GetAwaiter().GetResult();
            _cacheManagementService = cacheManagementService;
        }

        public async Task PerformMotionAsync(int atpThreshold)
        {
            await _cacheManagementService.PerformFunctionAsync(async (cell) =>
            {
                PerformMotionAndRespire(cell, atpThreshold);
                await Task.CompletedTask;
            });
        }

        private void PerformMotionAndRespire(Myocyte myocyte, int atpThreshold)
        {
            while (myocyte.ATPCount < atpThreshold)
            {
                Console.WriteLine("ATP count insufficient, performing respiration.");
                _respirationService.Process();
            }

            myocyte.ATPCount -= atpThreshold;
            Console.WriteLine($"Motion performed, {atpThreshold} ATP consumed");
        }
    }
}
