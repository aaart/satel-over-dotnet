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
        private readonly int _iterationCount;
        private int _iteration;

        public TaskPlanner(int iterationCount)
        {
            _iterationCount = iterationCount;
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

                if (++_iteration == _iterationCount)
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