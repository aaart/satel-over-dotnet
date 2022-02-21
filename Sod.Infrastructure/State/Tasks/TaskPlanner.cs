using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public class TaskPlanner : LoggingCapability, ITaskPlanner
    {
        public Task Plan(ITaskQueue queue, int iteration)
        {
            if (iteration == 0)
            {
                queue.EnqueueAsync(new SatelTask(TaskType.ReadOutputs));
                queue.EnqueueAsync(new SatelTask(TaskType.ReadInputs));
            }

            return Task.CompletedTask;
        }
    }
}