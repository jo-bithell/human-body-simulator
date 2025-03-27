using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Workers;

namespace Lungs
{
    public class AirDiffusionWorker : BaseWorker
    {
        private readonly Air _air;
        private readonly List<AlveolarCell> _alveolarCells;

        public AirDiffusionWorker(Air air, List<AlveolarCell> cells)
        {
            _air = air;
            _alveolarCells = cells;
        }

        public override async Task PerformAction(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DiffuseGases(_air);

                Console.WriteLine("Diffused gases");

                await Task.Delay(1000, cancellationToken);
            }
        }

        private void DiffuseGases(Air air)
        {
            foreach (var cell in _alveolarCells)
            {
                cell.DiffuseGases(air);
            }
        }
    }
}