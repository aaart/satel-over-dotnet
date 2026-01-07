using Microsoft.Extensions.Logging;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Handlers.Impl;

public class ActualStateChangedNotificationTaskHandler(IOutgoingEventPublisher eventPublisher)
    : BaseHandler<ActualStateChangedNotificationTask>
{
    protected override async Task<IEnumerable<BaseSatelTask>> Handle(ActualStateChangedNotificationTask data)
    {
        foreach (var state in data.Notifications)
        {
            Logger.LogInformation($"Outgoing event type is {data.OutgoingEventType.ToString()}. index: {state.Index}, value: {state.Value}");
            await eventPublisher.PublishAsync(new OutgoingEvent(data.OutgoingEventType, state.Index, state.Value));
        }

        return Enumerable.Empty<BaseSatelTask>();
    }
}