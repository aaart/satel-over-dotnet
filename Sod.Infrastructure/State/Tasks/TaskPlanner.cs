using System;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;
using Task = System.Threading.Tasks.Task;

namespace Sod.Infrastructure.State.Tasks
{
    public class TaskPlanner : LoggingCapability, ITaskPlanner
    {
        private readonly int _maxIterationCount;
        private int _iteration;

        public TaskPlanner(int maxIterationCount)
        {
            _maxIterationCount = maxIterationCount;
        }

        public Task Plan(ITaskQueue queue)
        {
            try
            {
                if (_iteration == 0)
                {
                    queue.EnqueueAsync(new SatelTask(TaskType.ReadOutputs));
                    queue.EnqueueAsync(new SatelTask(TaskType.ReadInputs));
                }

                if (++_iteration == _maxIterationCount)
                {
                    _iteration = 0;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                _iteration = 0;
            }

            return Task.CompletedTask;
        }
    }
}