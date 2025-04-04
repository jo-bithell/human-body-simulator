using System.Collections.Concurrent;

namespace SharedLogic.Messaging
{
    public class SnapshotCache<T>
    {
        public ConcurrentQueue<T> Queue { get; set; } = [];
    }
}