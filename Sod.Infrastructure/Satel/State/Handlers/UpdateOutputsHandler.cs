﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class UpdateOutputsHandler : LoggingCapability, IHandler
    {
        private readonly ITaskQueue _queue;
        private readonly IManipulator _manipulator;

        public UpdateOutputsHandler(ITaskQueue queue, IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(UpdateOutputsHandler)} is executing.");
            _queue = queue;
            _manipulator = manipulator;
        }


        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            var disableOutputs = new bool[128];
            var enableOutputs = new bool[128];
            var disableChanges = new Dictionary<string, object>();
            var enableChanges = new Dictionary<string, object>();
            foreach (var parameter in parameters)
            {
                var index = Convert.ToInt32(parameter.Key) - 1;
                var enable = OnOffParse.ToBoolean(parameter.Value.ToString()!);
                if (enable)
                {
                    enableOutputs[index] = true;
                    enableChanges.Add($"{Convert.ToString(index + 1)}", true);
                }
                else
                {
                    disableOutputs[index] = true;
                    disableChanges.Add($"{Convert.ToString(index + 1)}", false);
                }
            }
            
            if (enableChanges.Any())
            {
                await _manipulator.EnableOutputs(enableOutputs);
                await _queue.Enqueue(new SatelTask(TaskType.NotifyOutputsChanged, enableChanges));
            }

            if (disableChanges.Any())
            {
                await _manipulator.DisableOutputs(disableOutputs);
                await _queue.Enqueue(new SatelTask(TaskType.NotifyOutputsChanged, disableChanges));
            }
        }
    }
}