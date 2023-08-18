using System.Collections.Concurrent;
using Sod.Model.Tasks;

namespace Sod.Model.DataStructures;

public class InMemoryTaskQueue : ITaskQueue
{
    private readonly ConcurrentQueue<SatelTask> _queue = new();

    public Task EnqueueAsync(SatelTask satelTask)
    {
        _queue.Enqueue(satelTask);
        return Task.CompletedTask;
    }

    public Task<(bool exists, SatelTask? value)> DequeueAsync()
    {
        return _queue.TryDequeue(out var value) ? Task.FromResult((true, (SatelTask?)value)) : Task.FromResult((false, (SatelTask?)null));
    }

    public Task Clear()
    {
        return Task.Run(() => _queue.Clear());
    }
}