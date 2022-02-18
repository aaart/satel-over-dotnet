using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public interface IQueueProcessor
    {
        Task Process(ITaskQueue queue);
    }
}