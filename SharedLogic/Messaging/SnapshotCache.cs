using System.Collections.Concurrent;

namespace KafkaCommon
{
    public class SnapshotCache<T>
    {
        public ConcurrentQueue<T> Queue { get; set; } = [];
    }
}