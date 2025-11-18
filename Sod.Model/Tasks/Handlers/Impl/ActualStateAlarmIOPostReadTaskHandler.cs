using Microsoft.Extensions.Logging;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Handlers.Impl;

public class ActualStateAlarmIOPostReadTaskHandler : BaseHandler<ActualStateAlarmIOPostReadTask>
{
    protected override Task<IEnumerable<BaseSatelTask>> Handle(ActualStateAlarmIOPostReadTask data)
    {
        var tasks = new List<BaseSatelTask>();
        var anyPartitionArmed = data.ActualState.Any(x => x);

        if (data.Changes.Any())
        {
            Logger.LogInformation($"{data.Changes.Count} change(s) found. {data.OutgoingEventType} event will be send.");
            var t1 = new PersistedStateUpdateTask(data.PersistedStateKey, data.ActualState);
            var t2 = new ActualStateChangedNotificationTask(data.Changes, data.OutgoingEventType);
            tasks.Add(t1);
            tasks.Add(t2);
        }

        if (data.Changes.Any() || anyPartitionArmed)
            tasks.Add(
                new ActualStateBinaryIOReadTask(
                    Constants.Store.TriggeredPartitions,
                    IOBinaryReadType.AlarmTriggered,
                    OutgoingEventType.ArmedPartitionsStateChanged));

        return Task.FromResult(tasks.AsEnumerable());
    }
}