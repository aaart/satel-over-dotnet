using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateChangedNotificationTask(
    IEnumerable<BinaryIOState> notifications,
    OutgoingEventType outgoingEventType)
    : SatelTask
{
    public IEnumerable<BinaryIOState> Notifications { get; } = notifications;
    public OutgoingEventType OutgoingEventType { get; } = outgoingEventType;
}