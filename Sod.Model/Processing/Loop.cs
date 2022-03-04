using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;

namespace Sod.Model.Processing
{
    public class Loop : LoggingCapability
    {
        private readonly IQueueProcessor _processor;
        private readonly ITaskPlanner _planner;
        private readonly ITaskQueue _queue;
        private readonly LoopOptions _options;

        public Loop(
            IQueueProcessor processor,
            ITaskPlanner planner,
            ITaskQueue queue,
            LoopOptions opt)
        {
            _processor = processor;
            _planner = planner;
            _queue = queue;
            _options = opt;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var iteration = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _planner.Plan(_queue, iteration);
                    await _processor.Process(_queue);
                    await Task.Delay(_options.Interval, stoppingToken);
                    if (iteration == _options.IterationCount)
                    {
                        iteration = 0;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, e.Message);
                }
            }
        }
    }
}