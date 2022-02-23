using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.State.Handlers.Notifications
{
    public class InputsChangeNotificationStateHandler : ChangeNotificationStateHandler
    {
        public InputsChangeNotificationStateHandler(IOutgoingEventPublisher eventPublisher) 
            : base(eventPublisher)
        {
        }

        protected override OutgoingEventType OutgoingEventType => OutgoingEventType.InputsStateChanged;
    }
}