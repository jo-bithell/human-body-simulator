using SharedLogic.Respiration.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services;

namespace SharedLogic.Respiration.Factories
{
    public class RespirationProcessorFactory<C> : IRespirationProcessorFactory<C> where C : Cell
    {
        public IRespirationProcessor<C> GetServiceForRespiration(C cell)
        {
            var respirationService = GetRespirationService(cell);
            return respirationService;
        }

        private IRespirationProcessor<C> GetRespirationService(C cell)
        {
            if (cell.GlucoseCount > 0 && cell.OxygenCount > 0)
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new GlucoseRespirationProcessor<C>(cell);
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }
    }
}
