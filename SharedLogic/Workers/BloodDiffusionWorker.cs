using KafkaCommon;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Workers;

namespace SharedLogic
{
    public class BloodDiffusionWorker<C> : BaseWorker where C : Cell
    {
        private readonly SnapshotCache<Blood> _bloodCacheCache;
        private readonly List<C> _cells;

        public BloodDiffusionWorker(SnapshotCache<Blood> bloodCache, List<C> cells)
        {
            _bloodCacheCache = bloodCache;
            _cells = cells;
        }

        public override async Task PerformAction(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_bloodCacheCache.Queue.TryDequeue(out var blood))
                {
                    await Task.Delay(1000, cancellationToken);
                    DiffuseNutrients(blood);
                    Console.WriteLine("Diffused nutrients to blood");
                    _bloodCacheCache.Queue.Enqueue(blood);
                }
            }
        }

        private void DiffuseNutrients(Blood blood)
        {
            foreach (var cell in _cells)
            {
                cell.DiffuseNutrients(blood);
            }
        }
    }
}