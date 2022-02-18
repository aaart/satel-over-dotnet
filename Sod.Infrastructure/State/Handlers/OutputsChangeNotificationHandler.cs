using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.State.Handlers
{
    public class OutputsChangeNotificationHandler : ChangeNotificationHandler
    {
        public OutputsChangeNotificationHandler(IOutgoingEventPublisher eventPublisher) 
            : base(eventPublisher)
        {
        }

        protected override OutgoingEventType OutgoingEventType => OutgoingEventType.OutputsStateChanged;
    }
}