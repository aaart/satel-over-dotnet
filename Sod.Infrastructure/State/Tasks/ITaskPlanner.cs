using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public interface ITaskPlanner
    {
        Task Plan(ITaskQueue queue, int iteration);
    }
}