using Quartz;
using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Jobs
{
    public class RespirationJob<C> : IJob where C : Cell
    {
        private readonly IRespirationService<C> _respirationService;

        public RespirationJob(IRespirationService<C> respirationService)
        {
            _respirationService = respirationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _respirationService.Process();
            await Task.CompletedTask;
        }
    }
}