using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace Lungs
{
    internal class AirDiffusionWorker : IJob
    {
        private readonly Air _air;
        private readonly List<AlveolarCell> _alveolarCells;

        public AirDiffusionWorker(Air air, List<AlveolarCell> cells)
        {
            _air = air;
            _alveolarCells = cells;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var cell in _alveolarCells)
            {
                cell.DiffuseGases(_air);
            }
            Console.WriteLine("Diffused gases");
            await Task.CompletedTask;
        }
    }
}