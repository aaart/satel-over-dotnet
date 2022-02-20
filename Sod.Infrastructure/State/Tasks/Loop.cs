using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Configuration;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
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
            IOptions<LoopOptions> opt)
        {
            _processor = processor;
            _planner = planner;
            _queue = queue;
            _options = opt.Value;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _planner.Plan(_queue);
                    await _processor.Process(_queue);
                    await Task.Delay(_options.Interval, stoppingToken);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, e.Message);
                }
            }
        }
    }
}