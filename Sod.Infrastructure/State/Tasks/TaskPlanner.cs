using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes;
using Sod.Infrastructure.Storage.TaskTypes.IOStateRead;

namespace Sod.Infrastructure.State.Tasks
{
    public class TaskPlanner : LoggingCapability, ITaskPlanner
    {
        public Task Plan(ITaskQueue queue, int iteration)
        {
            if (iteration == 0)
            {
                queue.EnqueueAsync(
                        new ReadStateTask(
                            Constants.Store.InputsState,
                            TaskType.NotifyIOChanged,
                            IOReadManipulatorMethod.Inputs,
                            OutgoingEventType.InputsStateChanged
                        ));
                queue.EnqueueAsync(
                        new ReadStateTask(
                            Constants.Store.OutputsState,
                            TaskType.NotifyIOChanged,
                            IOReadManipulatorMethod.Outputs,
                            OutgoingEventType.OutputsStateChanged
                        ));
            }

            return Task.CompletedTask;
        }
    }
}