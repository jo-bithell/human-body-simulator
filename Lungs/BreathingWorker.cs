using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace Lungs
{
    internal class BreathingWorker : IJob
    {
        private readonly int _atpThreshold = 5;
        private readonly List<Myocyte> _lungCells;
        private readonly Air _air;

        public BreathingWorker(List<Myocyte> lungCells, Air air)
        {
            _lungCells = lungCells;
            _air = air;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Pretend this intakes oxygen
            PerformMotion();
            RefreshAirInLungs();
            Console.WriteLine("Blood oxygenated");

            await Task.CompletedTask;
        }

        private void PerformMotion()
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