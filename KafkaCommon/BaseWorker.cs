using BiologicalComponents;
using Microsoft.Extensions.Hosting;

namespace KafkaCommon
{
    public class BaseWorker<T> : IHostedService
    {
        private readonly MessagePublisher<T> _producerService;
        private readonly SnapshotCache<T> _snapshotCache;
        private Task? _backgroundTask;
        private CancellationTokenSource? _cancellationTokenSource;

        public BaseWorker(MessagePublisher<T> producerService, SnapshotCache<T> snapshotCache)
        {
            _producerService = producerService;
            _snapshotCache = snapshotCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _backgroundTask = Task.Run(() => PollQueue(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            return Task.CompletedTask;
        }

        public virtual async Task PollQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_snapshotCache.Queue.TryDequeue(out var item))
                {
                    await _producerService.SendMessage(item);
                }

                // Delay to avoid busy-waiting
                await Task.Delay(1000, cancellationToken); // Poll every second
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource?.Cancel();

            if (_backgroundTask != null)
            {
                return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

            return Task.CompletedTask;
        }
    }
}