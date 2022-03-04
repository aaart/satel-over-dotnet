using System.Collections.Generic;
using Sod.Model.CommonTypes;

namespace Sod.Model.Tasks.Types
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