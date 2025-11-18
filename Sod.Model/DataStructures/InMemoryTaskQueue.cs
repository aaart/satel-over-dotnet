using System.Collections.Concurrent;
using Sod.Model.Tasks;

namespace Sod.Model.DataStructures;

public class InMemoryTaskQueue : ITaskQueue
{
    private readonly ConcurrentQueue<BaseSatelTask> _queue = new();

    public Task EnqueueAsync(BaseSatelTask satelTask)
    {
        _queue.Enqueue(satelTask);
        return Task.CompletedTask;
    }

    public Task<(bool exists, BaseSatelTask? value)> DequeueAsync()
    {
        return _queue.TryDequeue(out var value) ? Task.FromResult((true, (BaseSatelTask?)value)) : Task.FromResult((false, (BaseSatelTask?)null));
    }

    public Task Clear()
    {
        return Task.Run(() => _queue.Clear());
    }
}