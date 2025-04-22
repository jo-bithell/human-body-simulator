using SharedLogic.Respiration.Factories;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class RespirationService
    {
        private readonly IRespirationService _respirationService;

        internal RespirationService(IRespirationServiceFactory respirationServiceFactory)
        {
            _respirationService = respirationServiceFactory.GetServiceForRespiration().GetAwaiter().GetResult();
        }

        internal void PerformRespiration()
        {
            _respirationService.Process();
            Console.WriteLine("Performed respiration.");
        }
    }
}
