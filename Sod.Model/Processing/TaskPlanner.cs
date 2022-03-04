using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Processing
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