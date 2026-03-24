using Microsoft.Extensions.Logging;
using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Factories
{
    public class RespirationProcessorFactory<C> : IRespirationProcessorFactory<C> where C : Cell
    {
        private readonly RespirationTypeSelectorService<C> _respirationTypeSelectorService;
        private readonly ILogger<RespirationProcessorFactory<C>> _logger;

        public RespirationProcessorFactory(RespirationTypeSelectorService<C> respirationTypeSelectorService, ILogger<RespirationProcessorFactory<C>> logger)
        {
            _respirationTypeSelectorService = respirationTypeSelectorService;
            _logger = logger;
        }

        public IRespirationProcessor<C> GetServiceForRespiration(C cell)
        {
            if (_respirationTypeSelectorService.CanDoAerobicGlucoseRespiration(cell))
            {
                _logger.LogInformation("Aerobic glucose metabolism selected.");
                return new GlucoseRespirationProcessor<C>(cell);
            }

            if (_respirationTypeSelectorService.CanDoLipidRespiration(cell))
            {
                _logger.LogInformation("Lipid metabolism selected.");
                return new LipidRespirationProcessor<C>(cell);
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }
    }
}