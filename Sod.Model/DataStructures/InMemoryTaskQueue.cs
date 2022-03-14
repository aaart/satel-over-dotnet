using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Model.Tasks;

namespace Sod.Model.DataStructures
{
    public class InMemoryTaskQueue : ITaskQueue
    {
        private readonly Queue<SatelTask> _queue = new();

        public Task EnqueueAsync(SatelTask satelTask)
        {
            _queue.Enqueue(satelTask);
            return Task.CompletedTask;
        }

        public Task<(bool exists, SatelTask? value)> DequeueAsync() => 
            _queue.TryDequeue(out SatelTask? value) ? Task.FromResult((true, (SatelTask?)value)) : Task.FromResult((false, (SatelTask?)null));
    }
}