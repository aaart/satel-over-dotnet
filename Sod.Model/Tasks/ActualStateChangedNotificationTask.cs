using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks;

public class ActualStateChangedNotificationTask(
    IEnumerable<BinaryIOState> notifications,
    OutgoingEventType outgoingEventType)
    : BaseSatelTask
{
    public IEnumerable<BinaryIOState> Notifications { get; } = notifications;
    public OutgoingEventType OutgoingEventType { get; } = outgoingEventType;
}