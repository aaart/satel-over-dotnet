using System.Collections.Generic;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types
{
    public class ActualStateChangedNotificationTask : SatelTask
    {
        public ActualStateChangedNotificationTask(IEnumerable<IOState> notifications, OutgoingEventType outgoingEventType)
        {
            Notifications = notifications;
            OutgoingEventType = outgoingEventType;
        }

        public IEnumerable<IOState> Notifications { get; }
        public OutgoingEventType OutgoingEventType { get; }
    }
}