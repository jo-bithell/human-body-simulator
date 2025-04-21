using Quartz;
using SharedLogic.Diffusion.Services;
using SharedLogic.Models.Cells;

namespace SharedLogic.Diffusion.Jobs
{
    public class BloodDiffusionJob<C> : IJob where C : Cell
    {
        private readonly BloodDiffusionService<C> _diffusionService;

        public BloodDiffusionJob(BloodDiffusionService<C> diffusionService)
        {
            _diffusionService = diffusionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _diffusionService.DiffuseAsync();
            await Task.CompletedTask;
        }
    }
}