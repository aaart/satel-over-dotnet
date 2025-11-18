using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateBinaryIOPostReadTask(
    IList<BinaryIOState> changes,
    string persistedStateKey,
    bool[] actualState,
    OutgoingEventType outgoingEventType)
    : SatelTask
{
    public IList<BinaryIOState> Changes { get; } = changes;
    public string PersistedStateKey { get; } = persistedStateKey;
    public bool[] ActualState { get; } = actualState;
    public OutgoingEventType OutgoingEventType { get; } = outgoingEventType;
}