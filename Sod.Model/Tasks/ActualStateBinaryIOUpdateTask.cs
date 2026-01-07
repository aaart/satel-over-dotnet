using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks;

public class ActualStateBinaryIOUpdateTask(
    IEnumerable<BinaryIOState> updates,
    IOBinaryUpdateType method,
    bool notifyChanged,
    OutgoingEventType eventType,
    int outputCount)
    : BaseSatelTask
{
    public IEnumerable<BinaryIOState> Updates { get; } = updates;
    public IOBinaryUpdateType Method { get; } = method;
    public bool NotifyChanged { get; } = notifyChanged;
    public OutgoingEventType EventType { get; } = eventType;
    public int OutputCount { get; } = outputCount;
}