using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.State.Handlers
{
    public class OutputsChangeNotificationStateHandler : ChangeNotificationStateHandler
    {
        public OutputsChangeNotificationStateHandler(IOutgoingEventPublisher eventPublisher) 
            : base(eventPublisher)
        {
        }

        protected override OutgoingEventType OutgoingEventType => OutgoingEventType.OutputsStateChanged;
    }
}