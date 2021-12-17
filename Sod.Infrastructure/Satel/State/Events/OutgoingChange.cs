namespace Sod.Infrastructure.Satel.State.Events
{
    public record OutgoingChange
    {
        public OutgoingChange(EventType type, int reference, string value)
        {
            Type = type;
            Reference = reference;
            Value = value;
        }
        
        public EventType Type { get; }
        public int Reference { get; }
        public string Value { get; }
    }
}