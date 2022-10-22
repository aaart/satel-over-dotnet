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
                        new ActualStateBinaryIOReadTask(
                            Constants.Store.InputsState,
                            NotificationTaskType.NotifyIOChanged,
                            IOBinaryReadManipulatorMethod.Inputs,
                            OutgoingEventType.InputsStateChanged
                        ));
                queue.EnqueueAsync(
                        new ActualStateBinaryIOReadTask(
                            Constants.Store.OutputsState,
                            NotificationTaskType.NotifyIOChanged,
                            IOBinaryReadManipulatorMethod.Outputs,
                            OutgoingEventType.OutputsStateChanged
                        ));
                queue.EnqueueAsync(
                    new ActualStateBinaryIOReadTask(
                        Constants.Store.ArmedPartitions,
                        NotificationTaskType.NotifyArmedPartitionsChanged,
                        IOBinaryReadManipulatorMethod.ArmedPartitions,
                        OutgoingEventType.ArmedPartitionsStateChanged));
                
            }

            return Task.CompletedTask;
        }
    }
}