using System.Collections.Concurrent;

namespace SharedLogic.Messaging.Models
{
    public class SnapshotCache<T>
    {
        public ConcurrentQueue<T> Queue { get; set; } = [];
    }
}