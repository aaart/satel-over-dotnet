using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks;

namespace Sod.Model.Tasks.Handlers.Impl;

public class GlobalStateNotificationTaskHandler : BaseHandler<GlobalStateNotificationTask>
{
    private readonly IOutgoingEventPublisher _eventPublisher;

    public GlobalStateNotificationTaskHandler(IOutgoingEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task<IEnumerable<BaseSatelTask>> Handle(GlobalStateNotificationTask data)
    {
        await _eventPublisher.PublishAsync(new OutgoingEvent(data.OutgoingEventType, 0, data.JsonPayload));
        return Enumerable.Empty<BaseSatelTask>();
    }
}
