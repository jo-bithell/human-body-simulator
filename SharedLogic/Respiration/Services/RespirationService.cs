using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class RespirationService
    {
        private readonly IRespirationService _metaboliser;

        internal RespirationService(IRespirationService metaboliser)
        {
            _metaboliser = metaboliser;
        }

        internal void PerformRespiration()
        {
            _metaboliser.Process();
            Console.WriteLine("Performed respiration.");
        }
    }
}
