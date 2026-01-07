namespace Sod.Model.Events.Outgoing;

public record FailedOutgoingEvent(OutgoingEvent OutgoingEvent, FailedOutgoingEventReason Reason)
    : OutgoingEvent(OutgoingEvent.Type, OutgoingEvent.Reference, OutgoingEvent.Value);
