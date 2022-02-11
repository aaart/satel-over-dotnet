using System.Threading.Tasks;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;
using Task = System.Threading.Tasks.Task;

namespace Sod.Infrastructure.Satel.State.Tasks
{
    public class TaskPlanner : ITaskPlanner
    {
        public Task Plan(ITaskQueue queue)
        {
            queue.Enqueue(new SatelTask(TaskType.ReadOutputs));
            queue.Enqueue(new SatelTask(TaskType.ReadInputs));
            return Task.CompletedTask;
        }
    }
}