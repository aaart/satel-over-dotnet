using System.Threading.Tasks;
using Sod.Model.Tasks;

namespace Sod.Model.DataStructures
{
    public interface ITaskQueue
    {
        Task EnqueueAsync(SatelTask satelTask);
        Task<(bool exists, SatelTask? value)> DequeueAsync();
    }
}