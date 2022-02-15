using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.State.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Worker
{
    public class Worker : BackgroundService, ILoggingCapability
    {
        private readonly IQueueSubscription _subscription;
        private readonly ITaskPlanner _planner;
        private readonly ITaskQueue _queue;
        private readonly int _interval = 100;

        public Worker(
            IQueueSubscription subscription,
            ITaskPlanner planner,
            ITaskQueue queue)
        {
            _subscription = subscription;
            _planner = planner;
            _queue = queue;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _planner.Plan(_queue);
                    await _subscription.ReceiveTasks(_queue);
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