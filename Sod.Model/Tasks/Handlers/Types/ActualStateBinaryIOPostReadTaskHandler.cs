using Microsoft.Extensions.Logging;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateBinaryIOPostReadTaskHandler : BaseHandler<ActualStateBinaryIOPostReadTask>
{
    protected override Task<IEnumerable<SatelTask>> Handle(ActualStateBinaryIOPostReadTask data)
    {
        if (data.Changes.Any())
        {
            Logger.LogInformation($"{data.Changes.Count} changes found. {data.OutgoingEventType} event will be send.");
            var t1 = new PersistedStateUpdateTask(data.PersistedStateKey, data.ActualState);
            var t2 = new ActualStateChangedNotificationTask(data.Changes, data.OutgoingEventType); 
            return Task.FromResult(new SatelTask[] { t1, t2 }.AsEnumerable());
        }
        
        return Task.FromResult(Enumerable.Empty<SatelTask>());
    }
}