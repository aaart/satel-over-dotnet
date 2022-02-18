using System.Threading.Tasks;

namespace Sod.Infrastructure.Storage
{
    public interface ITaskQueue
    {
        Task EnqueueAsync(SatelTask satelTask);
        Task<(bool exists, SatelTask? value)> DequeueAsync();
    }
}