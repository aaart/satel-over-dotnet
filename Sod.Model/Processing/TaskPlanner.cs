using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
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
                        new ActualStateReadTask(
                            Constants.Store.InputsState,
                            NotificationTaskType.NotifyIOChanged,
                            IOReadManipulatorMethod.Inputs,
                            OutgoingEventType.InputsStateChanged
                        ));
                queue.EnqueueAsync(
                        new ActualStateReadTask(
                            Constants.Store.OutputsState,
                            NotificationTaskType.NotifyIOChanged,
                            IOReadManipulatorMethod.Outputs,
                            OutgoingEventType.OutputsStateChanged
                        ));
                queue.EnqueueAsync(
                    new ActualStateReadTask(
                        Constants.Store.ArmedPartitions,
                        NotificationTaskType.NotifyArmedPartitionsChanged,
                        IOReadManipulatorMethod.ArmedPartitions,
                        OutgoingEventType.ArmedPartitionsStateChanged));
                queue.EnqueueAsync(
                    new PersistedStateAlarmStateReadTask());
            }

            return Task.CompletedTask;
        }
    }
}