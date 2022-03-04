using System;
using System.Collections.Generic;
using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.Storage.TaskTypes.Notifications
{
    public class IOChangeNotificationTask : SatelTask
    {
        public IOChangeNotificationTask(IEnumerable<IOState> notifications, OutgoingEventType outgoingEventType)
        {
            Notifications = notifications;
            OutgoingEventType = outgoingEventType;
        }

        public IEnumerable<IOState> Notifications { get; }
        public OutgoingEventType OutgoingEventType { get; }
    }
}