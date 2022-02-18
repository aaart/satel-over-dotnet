using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.State.Handlers
{
    public class InputsChangeNotificationHandler : ChangeNotificationHandler
    {
        public InputsChangeNotificationHandler(IOutgoingEventPublisher eventPublisher) 
            : base(eventPublisher)
        {
        }

        protected override OutgoingEventType OutgoingEventType => OutgoingEventType.InputsStateChanged;
    }
}