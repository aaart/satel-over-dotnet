namespace Sod.Infrastructure.Satel.State.Events.Outgoing
{
    public record OutgoingEvent
    {
        public OutgoingEvent(OutgoingEventType type, int reference, string value)
        {
            Type = type;
            Reference = reference;
            Value = value;
        }
        
        public OutgoingEventType Type { get; }
        public int Reference { get; }
        public string Value { get; }
    }
}