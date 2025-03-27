using Microsoft.Extensions.Hosting;

namespace SharedLogic.Workers
{
    public class BaseWorker : IHostedService
    {
        private Task? _backgroundTask;
        private CancellationTokenSource? _cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _backgroundTask = Task.Run(() => PerformAction(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            return Task.CompletedTask;
        }

        public virtual async Task PerformAction(CancellationToken cancellationToken)
        {
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