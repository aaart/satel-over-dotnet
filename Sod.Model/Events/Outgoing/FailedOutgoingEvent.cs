using Sod.Model.CommonTypes;

namespace Sod.Model.Events.Outgoing
{
    public class FailedOutgoingEvent : OutgoingEvent
    {
        public FailedOutgoingEvent(OutgoingEvent outgoingEvent) 
            : base(outgoingEvent.Type, outgoingEvent.Reference, outgoingEvent.Value)
        {
        }
    }
}