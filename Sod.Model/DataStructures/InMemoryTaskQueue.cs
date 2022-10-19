using System.Collections.Concurrent;
using System.Threading.Tasks;
using Sod.Model.Tasks;

namespace Sod.Model.DataStructures
{
    public class InMemoryTaskQueue : ITaskQueue
    {
        private readonly ConcurrentStack<SatelTask> _queue = new();

        public Task EnqueueAsync(SatelTask satelTask)
        {
            _queue.Push(satelTask);
            return Task.CompletedTask;
        }

        public Task<(bool exists, SatelTask? value)> DequeueAsync() => 
            _queue.TryPeek(out SatelTask? value) ? Task.FromResult((true, (SatelTask?)value)) : Task.FromResult((false, (SatelTask?)null));
    }
}