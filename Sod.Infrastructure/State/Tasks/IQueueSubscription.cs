using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public interface IQueueSubscription
    {
        Task ReceiveTasks(ITaskQueue queue);
    }
}