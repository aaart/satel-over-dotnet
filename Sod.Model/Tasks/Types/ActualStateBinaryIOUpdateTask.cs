using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateBinaryIOUpdateTask : SatelTask
{
    public ActualStateBinaryIOUpdateTask(IEnumerable<BinaryIOState> updates, IOBinaryUpdateType method, bool notifyChanged, OutgoingEventType eventType, int outputCount)
    {
        Updates = updates;
        Method = method;
        NotifyChanged = notifyChanged;
        EventType = eventType;
        OutputCount = outputCount;
    }

    public IEnumerable<BinaryIOState> Updates { get; }
    public IOBinaryUpdateType Method { get; }
    public bool NotifyChanged { get; }
    public OutgoingEventType EventType { get; }
    public int OutputCount { get; }
}