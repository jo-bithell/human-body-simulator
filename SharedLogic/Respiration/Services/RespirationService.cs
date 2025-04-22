using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Factories;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class RespirationService<C> : IRespirationService<C> where C : Cell
    {
        private readonly ICacheManagementService<C> _cacheManagementService;
        private readonly IRespirationProcessorFactory<C> _respirationProcessorFactory;
        public RespirationService(IRespirationProcessorFactory<C> respirationProcessorFactory, ICacheManagementService<C> cacheManagementService)
        {
            _respirationProcessorFactory = respirationProcessorFactory;
            _cacheManagementService = cacheManagementService;
        }

        public async Task Process()
        {
            await _cacheManagementService.PerformFunctionAsync(async (C c) =>
            {
                PerformRespiration(c);
                await Task.CompletedTask;
            });

        }

        private void PerformRespiration(C c)
        {
            var respirationProcessor = _respirationProcessorFactory.GetServiceForRespiration(c);
            respirationProcessor.Process();
        }
    }
}
