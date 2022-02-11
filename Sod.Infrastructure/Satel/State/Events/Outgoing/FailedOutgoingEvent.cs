namespace Sod.Infrastructure.Satel.State.Events.Outgoing
{
    public class FailedOutgoingEvent : OutgoingEvent
    {
        public FailedOutgoingEvent(OutgoingEvent outgoingEvent) 
            : base(outgoingEvent.Type, outgoingEvent.Reference, outgoingEvent.Value)
        {
        }
    }
}