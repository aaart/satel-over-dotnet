using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Configuration;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;
using Task = System.Threading.Tasks.Task;

namespace Sod.Infrastructure.State.Tasks
{
    public class TaskPlanner : LoggingCapability, ITaskPlanner
    {
        private int _iteration;
        private readonly TaskPlannerOptions _options;

        public TaskPlanner(IOptions<TaskPlannerOptions> opt)
        {
            _options = opt.Value;
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

                if (++_iteration == _options.IterationCount)
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