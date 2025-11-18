using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks;

public class ActualStateBinaryIOReadTask(
    string persistedStateKey,
    IOBinaryReadType method,
    OutgoingEventType outgoingEventType)
    : BaseSatelTask
{
    public string PersistedStateKey { get; } = persistedStateKey;
    public IOBinaryReadType Method { get; } = method;
    public OutgoingEventType OutgoingEventType { get; } = outgoingEventType;
}