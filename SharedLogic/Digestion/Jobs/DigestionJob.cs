using Quartz;
using SharedLogic.Digestion.Services.Interfaces;

namespace SharedLogic.Digestion.Jobs
{
    public class DigestionJob : IJob
    {
        private readonly IDigestionService _csvService;

        public DigestionJob(IDigestionService digestionService)
        {
            _csvService = digestionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _csvService.DigestAsync();
            await Task.CompletedTask;
        }
    }
}