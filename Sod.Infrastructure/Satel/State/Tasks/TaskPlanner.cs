using System.Threading.Tasks;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;
using Task = System.Threading.Tasks.Task;

namespace Sod.Infrastructure.Satel.State.Tasks
{
    public class TaskPlanner : ITaskPlanner
    {
        private readonly int _maxIterationCount;
        private int _iteration = 0;

        public TaskPlanner(int maxIterationCount)
        {
            _maxIterationCount = maxIterationCount;
        }
        
        public Task Plan(ITaskQueue queue)
        {
            if (_iteration == 0)
            {
                queue.Enqueue(new SatelTask(TaskType.ReadOutputs));
                queue.Enqueue(new SatelTask(TaskType.ReadInputs));
            }

            if (++_iteration == _maxIterationCount)
            {
                _iteration = 0;
            }
            
            return Task.CompletedTask;
        }
    }
}