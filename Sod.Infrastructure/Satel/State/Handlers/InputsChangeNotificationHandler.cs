using Sod.Infrastructure.Satel.State.Events.Outgoing;

namespace Sod.Infrastructure.Satel.State.Handlers
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