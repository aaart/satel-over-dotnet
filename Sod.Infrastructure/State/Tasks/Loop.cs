using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public class Loop : LoggingCapability
    {
        private readonly IQueueProcessor _processor;
        private readonly ITaskPlanner _planner;
        private readonly ITaskQueue _queue;
        private readonly int _interval = 100;

        public Loop(
            IQueueProcessor processor,
            ITaskPlanner planner,
            ITaskQueue queue)
        {
            _processor = processor;
            _planner = planner;
            _queue = queue;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _planner.Plan(_queue);
                    await _processor.Process(_queue);
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, e.Message);
                }
            }
        }
    }
}