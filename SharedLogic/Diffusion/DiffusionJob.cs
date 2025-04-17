using Quartz;

namespace SharedLogic.Diffusion
{
    public class DiffusionJob : IJob
    {
        private readonly List<IDiffusionService> _diffusionServices;

        public DiffusionJob(BloodDiffusionServiceFactory diffusionServiceFactory)
        {
            _diffusionServices = diffusionServiceFactory.Create();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var diffusionService in _diffusionServices)
            {
                await diffusionService.Diffuse();
            }

            await Task.CompletedTask;
        }
    }
}