using Lungs.Services.Interfaces;
using Lungs.Models;
using SharedLogic.Motion.Services;

namespace Lungs.Services
{
    internal class OxygenAirRefreshService : IOxygenAirRefreshService
    {
        private readonly IMotionService _motionService;
        private readonly Air _air;
        private readonly int _atpThreshold = 5;

        public OxygenAirRefreshService(Air air, IMotionService motionService)
        {
            _air = air;
            _motionService = motionService;
        }

        public async Task PerformMotionAndRefreshAirAsync()
        {
            if (!await _motionService.CanPerformMotionAsync(_atpThreshold))
            {
                Console.WriteLine("Insufficient ATP to refresh air in lungs.");
                return;
            }

            await _motionService.PerformMotionAsync(_atpThreshold);
            RefreshAirInLungs();
            Console.WriteLine("Refreshed air in lungs.");
        }

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}
