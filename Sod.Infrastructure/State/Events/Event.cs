namespace Sod.Infrastructure.State.Events
{
    public record Event : IEvent
    {
        public Event(EventType type, object data)
        {
            Type = type;
            Data = data;
        }
        
        public EventType Type { get; }
        public object Data { get; }
    }
}