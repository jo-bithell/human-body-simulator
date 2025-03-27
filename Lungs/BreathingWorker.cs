using KafkaCommon;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Workers;

namespace Lungs
{
    public class BreathingWorker : BaseRespirationWorker
    {
        private readonly int _atpThreshold = 5;
        private readonly List<Myocyte> _lungCells;
        private readonly Air _air;

        public BreathingWorker(MessagePublisher<Blood> producerService, List<Myocyte> lungCells, Air air)
        {
            _lungCells = lungCells;
            _air = air;
        }

        public override async Task PerformAction(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Pretend this intakes oxygen
                PerformMotion();
                RefreshAirInLungs();
                Console.WriteLine("Blood oxygenated");

                await Task.Delay(1000, cancellationToken);
            }
        }

        protected override void PerformMotion()
        {
            foreach (Myocyte lungCell in _lungCells)
            {
                lungCell.PerformMotion(_atpThreshold);
            }
        }

        private void RefreshAirInLungs()
        {
            _air.OxygenCount += 50;
        }
    }
}