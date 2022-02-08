using System.Threading.Tasks;

namespace Sod.Infrastructure.Storage
{
    public interface ITaskQueue
    {
        Task Enqueue(SatelTask satelTask);
        Task<(bool exists, SatelTask? value)> Dequeue();
    }
}