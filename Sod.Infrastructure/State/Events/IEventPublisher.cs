namespace Sod.Infrastructure.State.Events
{
    public interface IEventPublisher
    {
        void Publish(IEvent evnt);
    }
}