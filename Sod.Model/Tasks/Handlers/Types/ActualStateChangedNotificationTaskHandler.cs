using Microsoft.Extensions.Logging;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateChangedNotificationTaskHandler : BaseHandler<ActualStateChangedNotificationTask>
{
    private readonly IOutgoingEventPublisher _eventPublisher;

    public ActualStateChangedNotificationTaskHandler(IOutgoingEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateChangedNotificationTask data)
    {
        foreach (var state in data.Notifications)
        {
            Logger.LogInformation($"Outgoing event type is {data.OutgoingEventType.ToString()}. index: {state.Index}, value: {state.Value}");
            await _eventPublisher.PublishAsync(new OutgoingEvent(data.OutgoingEventType, state.Index, state.Value));
        }

        return Enumerable.Empty<SatelTask>();
    }
}