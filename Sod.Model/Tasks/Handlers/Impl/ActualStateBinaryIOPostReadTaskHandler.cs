using Microsoft.Extensions.Logging;

namespace Sod.Model.Tasks.Handlers.Impl;

public class ActualStateBinaryIOPostReadTaskHandler : BaseHandler<ActualStateBinaryIOPostReadTask>
{
    protected override Task<IEnumerable<BaseSatelTask>> Handle(ActualStateBinaryIOPostReadTask data)
    {
        if (data.Changes.Any())
        {
            Logger.LogInformation($"{data.Changes.Count} change(s) found. {data.OutgoingEventType} event will be send.");
            var t1 = new PersistedStateUpdateTask(data.PersistedStateKey, data.ActualState);
            var t2 = new ActualStateChangedNotificationTask(data.Changes, data.OutgoingEventType);
            return Task.FromResult(new BaseSatelTask[] { t1, t2 }.AsEnumerable());
        }

        return Task.FromResult(Enumerable.Empty<BaseSatelTask>());
    }
}