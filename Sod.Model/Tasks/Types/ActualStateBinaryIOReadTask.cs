using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateBinaryIOReadTask(
    string persistedStateKey,
    NotificationTaskType notificationTaskType,
    IOBinaryReadType method,
    OutgoingEventType outgoingEventType)
    : SatelTask
{
    public string PersistedStateKey { get; } = persistedStateKey;
    //public NotificationTaskType NotificationTaskType { get; } = notificationTaskType;
    public IOBinaryReadType Method { get; } = method;
    public OutgoingEventType OutgoingEventType { get; } = outgoingEventType;
}