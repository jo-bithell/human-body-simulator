using Quartz;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace LeftAtrium
{
    internal class BloodProducerJob : IJob
    {
        private readonly SnapshotCache<Blood> _bloodCache;
        private readonly List<Myocyte> _myocytes;
        private readonly int _pumpCapacity = 100;
        private readonly int _atpThreshold = 5;

        internal BloodProducerJob(SnapshotCache<Blood> bloodCache, List<Myocyte> heartCells)
        {
            _bloodCache = bloodCache;
            _myocytes = heartCells;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            PerformMotion();

            for (int i = 0; i < _pumpCapacity; i ++)
            {
                while(_bloodCache.Queue.TryDequeue(out var blood))
                {
                    if (blood is not null)
                    {
                        var messagePublisher = GetMessagePublisher();
                        messagePublisher.SendMessage(blood);
                    }
                }
            }

            await Task.CompletedTask;
        }

        private void PerformMotion()
        {
            foreach (Myocyte myocyte in _myocytes)
            {
                myocyte.PerformMotion(_atpThreshold);
                Console.WriteLine("Heart cell performed motion");
            }
        }

        private MessagePublisher<Blood> GetMessagePublisher()
        {
            Random random = new Random();
            int randomValue = random.Next(1, 101);

            if (randomValue <= 20)
                return new MessagePublisher<Blood>("mouth");
            else if (randomValue <= 60)
                return new MessagePublisher<Blood>("small-intestine");
            else
                return new MessagePublisher<Blood>("stomach");
        }
    }
}