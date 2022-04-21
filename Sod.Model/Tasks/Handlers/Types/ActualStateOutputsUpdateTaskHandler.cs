using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class ActualStateOutputsUpdateTaskHandler : BaseHandler<ActualStateOutputsUpdateTask>
    {
        private readonly IManipulator _manipulator;

        public ActualStateOutputsUpdateTaskHandler(IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(ActualStateOutputsUpdateTaskHandler)} is executing.");
            _manipulator = manipulator;
        }


        protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateOutputsUpdateTask data)
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
                    tasks.Add(new ActualStateChangedNotificationTask(enableChanges, data.EventType));
                }
            }

            if (disableChanges.Any())
            {
                await _manipulator.DisableOutputs(disableOutputs);
                if (data.NotifyChanged)
                {
                    tasks.Add(new ActualStateChangedNotificationTask(disableChanges, data.EventType));
                }
            }

            return tasks;
        }
    }
}