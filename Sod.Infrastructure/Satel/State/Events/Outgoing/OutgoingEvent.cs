namespace Sod.Infrastructure.Satel.State.Events.Outgoing
{
    public class OutgoingEvent
    {
        public OutgoingEvent(OutgoingEventType type, int reference, string value)
        {
            Type = type;
            Reference = reference;
            Value = value;
        }
        
        public OutgoingEvent(OutgoingEventType type, int reference, bool value)
        {
            Type = type;
            Reference = reference;
            Value = OnOffParse.ToString(value);
        }
        
        public OutgoingEventType Type { get; }
        public int Reference { get; }
        public string Value { get; }
    }
}