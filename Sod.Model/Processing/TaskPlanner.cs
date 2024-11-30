using Sod.Infrastructure.Capabilities;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;
using Sod.Model.Tools;

namespace Sod.Model.Processing;

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
                    IOBinaryReadType.Inputs,
                    OutgoingEventType.InputsStateChanged
                ));
            queue.EnqueueAsync(
                new ActualStateBinaryIOReadTask(
                    Constants.Store.OutputsState,
                    NotificationTaskType.NotifyIOChanged,
                    IOBinaryReadType.Outputs,
                    OutgoingEventType.OutputsStateChanged
                ));
            queue.EnqueueAsync(
                new ActualStateBinaryIOReadTask(
                    Constants.Store.ArmedPartitions,
                    NotificationTaskType.NotifyArmedPartitionsChanged,
                    IOBinaryReadType.ArmedPartitions,
                    OutgoingEventType.ArmedPartitionsStateChanged));
            queue.EnqueueAsync(
                new ActualStateBinaryIOReadTask(
                    Constants.Store.SuppressedPartitions,
                    NotificationTaskType.NotifySuppressedPartitionsChanged,
                    IOBinaryReadType.SuppressedPartitions,
                    OutgoingEventType.SuppressedPartitionsStateChanged));
        }

        return Task.CompletedTask;
    }
}