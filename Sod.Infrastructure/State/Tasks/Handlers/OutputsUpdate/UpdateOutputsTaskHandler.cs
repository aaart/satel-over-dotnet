﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes.Notifications;
using Sod.Infrastructure.Storage.TaskTypes.OutputsUpdate;

namespace Sod.Infrastructure.State.Tasks.Handlers.OutputsUpdate
{
    public class UpdateOutputsTaskHandler : BaseHandler<OutputsUpdateTask>
    {
        private readonly IManipulator _manipulator;

        public UpdateOutputsTaskHandler(IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(UpdateOutputsTaskHandler)} is executing.");
            _manipulator = manipulator;
        }


        protected override async Task<IEnumerable<SatelTask>> Handle(OutputsUpdateTask data)
        {
            var disableOutputs = new bool[128];
            var enableOutputs = new bool[128];
            var disableChanges = new List<IOState>();
            var enableChanges = new List<IOState>();
            foreach (var state in data.Updates)
            {
                var index = state.Index - 1;
                var enable = state.Value;
                if (enable)
                {
                    enableOutputs[index] = true;
                    enableChanges.Add(state);
                }
                else
                {
                    disableOutputs[index] = true;
                    disableChanges.Add(state);
                }
            }

            var tasks = new List<SatelTask>();
            if (enableChanges.Any())
            {
                await _manipulator.EnableOutputs(enableOutputs);
                if (data.NotifyChanged)
                {
                    tasks.Add(new IOChangeNotificationTask(enableChanges, data.EventType));
                }
            }

            if (disableChanges.Any())
            {
                await _manipulator.DisableOutputs(disableOutputs);
                if (data.NotifyChanged)
                {
                    tasks.Add(new IOChangeNotificationTask(disableChanges, data.EventType));
                }
            }

            return tasks;
        }
    }
}