using KafkaCommon;
using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace SharedLogic
{
    public class BloodDiffusionWorker<C> : IJob where C : Cell
    {
        private readonly SnapshotCache<Blood> _bloodCacheCache;
        private readonly List<C> _cells;

        public BloodDiffusionWorker(SnapshotCache<Blood> bloodCache, List<C> cells)
        {
            _bloodCacheCache = bloodCache;
            _cells = cells;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            while (_bloodCacheCache.Queue.TryDequeue(out var blood))
            {
                DiffuseNutrients(blood);
                Console.WriteLine("Diffused nutrients to blood");
                _bloodCacheCache.Queue.Enqueue(blood);
            }

            await Task.CompletedTask;
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