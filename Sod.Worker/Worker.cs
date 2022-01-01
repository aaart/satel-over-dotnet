using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.State.Loop;

namespace Sod.Worker
{
    public class Worker : BackgroundService, ILoggingCapability
    {
        private readonly IStepCollectionFactory _stepCollectionFactory;

        public Worker(IStepCollectionFactory stepCollectionFactory)
        {
            _stepCollectionFactory = stepCollectionFactory;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var steps = _stepCollectionFactory.BuildStepCollection();
                    await steps.ExecuteAsync();
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, e.Message);
                }
            }
        }

        
    }
}