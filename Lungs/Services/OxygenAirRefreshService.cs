using Lungs.Services.Interfaces;
using Lungs.Models;
using SharedLogic.Motion.Services;
using Microsoft.Extensions.Logging;

namespace Lungs.Services
{
    internal class OxygenAirRefreshService : IOxygenAirRefreshService
    {
        private readonly IMotionService _motionService;
        private readonly Air _air;
        private readonly int _atpThreshold = 5;
        private readonly ILogger<OxygenAirRefreshService> _logger;

        public OxygenAirRefreshService(Air air, IMotionService motionService, ILogger<OxygenAirRefreshService> logger)
        {
            _air = air;
            _motionService = motionService;
            _logger = logger;
        }

        public async Task PerformMotionAndRefreshAirAsync()
        {
            if (!await _motionService.CanPerformMotionAsync(_atpThreshold))
            {
                _logger.LogWarning("Insufficient ATP to refresh air in lungs.");
                return;
            }

            await _motionService.PerformMotionAsync(_atpThreshold);
            RefreshAirInLungs();
            _logger.LogInformation("Refreshed air in lungs.");
        }

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}
