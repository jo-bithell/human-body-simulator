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
            await _motionService.PerformMotionAsync(_atpThreshold);
            RefreshAirInLungs();
        }

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}
