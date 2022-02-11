using Sod.Infrastructure.Satel.State.Events.Outgoing;

namespace Sod.Infrastructure.Satel.State.Handlers
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