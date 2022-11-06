using Microsoft.Extensions.Logging;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateAlarmIOPostReadTaskHandler : BaseHandler<ActualStateAlarmIOPostReadTask>
{
    protected override Task<IEnumerable<SatelTask>> Handle(ActualStateAlarmIOPostReadTask data)
    {
        var tasks = new List<SatelTask>();
        var anyPartitionArmed = data.ActualState.Any(x => x);
        
        if (data.Changes.Any())
        {
            Logger.LogInformation($"{data.Changes.Count} changes found. {data.OutgoingEventType} event will be send.");
            var t1 = new PersistedStateUpdateTask(data.PersistedStateKey, data.ActualState);
            var t2 = new ActualStateChangedNotificationTask(data.Changes, data.OutgoingEventType); 
            tasks.Add(t1);
            tasks.Add(t2);
        }

        if (data.Changes.Any() || anyPartitionArmed)
        {
            tasks.Add(
                new ActualStateBinaryIOReadTask(
                    Constants.Store.TriggeredPartitions, 
                    NotificationTaskType.NotifyAlarmTriggeredChanged, 
                    IOBinaryReadType.AlarmTriggered, 
                    OutgoingEventType.ArmedPartitionsStateChanged));
        }
        
        return Task.FromResult(tasks.AsEnumerable());
    }
}