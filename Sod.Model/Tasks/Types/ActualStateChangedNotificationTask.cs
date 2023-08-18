using System.Collections.Generic;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateChangedNotificationTask : SatelTask
{
    public ActualStateChangedNotificationTask(IEnumerable<BinaryIOState> notifications, OutgoingEventType outgoingEventType)
    {
        Notifications = notifications;
        OutgoingEventType = outgoingEventType;
    }

    public IEnumerable<BinaryIOState> Notifications { get; }
    public OutgoingEventType OutgoingEventType { get; }
}