using Sod.Model.CommonTypes;

namespace Sod.Model.Events.Outgoing;

public class FailedOutgoingEvent : OutgoingEvent
{
    public FailedOutgoingEvent(OutgoingEvent outgoingEvent, FailedOutgoingEventReason reason)
        : base(outgoingEvent.Type, outgoingEvent.Reference, outgoingEvent.Value)
    {
        Reason = reason;
    }

    private FailedOutgoingEventReason Reason { get; }
}