using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers.OutputsUpdate
{
    public class UpdateOutputsStateHandler : LoggingCapability, IStateHandler
    {
        private readonly ITaskQueue _queue;
        private readonly IManipulator _manipulator;

        public UpdateOutputsStateHandler(ITaskQueue queue, IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(UpdateOutputsStateHandler)} is executing.");
            _queue = queue;
            _manipulator = manipulator;
        }


        public async Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters)
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

            var tasks = new List<SatelTask>();
            if (enableChanges.Any())
            {
                await _manipulator.EnableOutputs(enableOutputs);
                tasks.Add(new SatelTask(TaskType.NotifyOutputsChanged, enableChanges));
            }

            if (disableChanges.Any())
            {
                await _manipulator.DisableOutputs(disableOutputs);
                tasks.Add(new SatelTask(TaskType.NotifyOutputsChanged, disableChanges));
            }

            return tasks;
        }
    }
}