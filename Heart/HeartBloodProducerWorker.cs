using KafkaCommon;
using Quartz;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace Heart
{
    public class HeartBloodProducerWorker : IJob
    {
        private readonly MessagePublisher<Blood> _producerService;
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly List<Myocyte> _myocytes;
        private readonly int _pumpCapacity = 100;
        private readonly int _atpThreshold = 5;

        public HeartBloodProducerWorker(MessagePublisher<Blood> producerService, SnapshotCache<Blood> bloodCache, List<Myocyte> heartCells)
        {
            _producerService = producerService;
            _bloodCache = bloodCache;
            _myocytes = heartCells;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            PopulateBloodCache();
            PerformMotion();

            for (int i = 0; i < _pumpCapacity; i ++)
            {
                while(_bloodCache.Queue.TryDequeue(out var blood))
                {
                    if (blood is not null)
                        _producerService.SendMessage(blood);
                }
            }

            await Task.CompletedTask;
        }

        private void PopulateBloodCache()
        {
            for (int i = 0; i < 5; i++)
            {
                Blood newBlood = new Blood();
                _bloodCache.Queue.Enqueue(newBlood);
            }
        }

        private void PerformMotion()
        {
            foreach (Myocyte myocyte in _myocytes)
            {
                myocyte.PerformMotion(_atpThreshold);
                Console.WriteLine("Heart cell performed motion");
            }
        }
    }
}