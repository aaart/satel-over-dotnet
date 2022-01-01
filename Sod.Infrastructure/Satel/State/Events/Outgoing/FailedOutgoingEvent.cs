namespace Sod.Infrastructure.Satel.State.Events.Outgoing
{
    public record FailedOutgoingEvent : OutgoingEvent
    {
        public FailedOutgoingEvent(OutgoingEvent outgoingEvent) 
            : base(outgoingEvent.Type, outgoingEvent.Reference, outgoingEvent.Value)
        {
        }
    }
}