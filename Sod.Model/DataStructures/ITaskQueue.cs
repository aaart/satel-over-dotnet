using Sod.Model.Tasks;

namespace Sod.Model.DataStructures;

public interface ITaskQueue
{
    Task EnqueueAsync(BaseSatelTask satelTask);
    Task<(bool exists, BaseSatelTask? value)> DequeueAsync();
    Task Clear();
}