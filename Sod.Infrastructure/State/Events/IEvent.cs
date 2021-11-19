namespace Sod.Infrastructure.State.Events
{
    public interface IEvent
    {
        EventType Type { get; }
        
        object Data { get; }
    }
}