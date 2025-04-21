using Lungs.Services.Interfaces;
using Quartz;

namespace Lungs.Jobs
{
    internal class OxygenAirRefreshJob : IJob
    {
        private readonly IOxygenAirRefreshService _oxygenAirRefreshService;

        public OxygenAirRefreshJob(IOxygenAirRefreshService oxygenAirRefreshService)
        {
            _oxygenAirRefreshService = oxygenAirRefreshService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _oxygenAirRefreshService.PerformMotionAndRefreshAirAsync();
            await Task.CompletedTask;
        }
    }
}