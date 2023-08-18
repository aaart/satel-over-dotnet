using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateBinaryIOPostReadTask : SatelTask
{
    public ActualStateBinaryIOPostReadTask(IList<BinaryIOState> changes, string persistedStateKey, bool[] actualState, OutgoingEventType outgoingEventType)
    {
        Changes = changes;
        PersistedStateKey = persistedStateKey;
        ActualState = actualState;
        OutgoingEventType = outgoingEventType;
    }

    public IList<BinaryIOState> Changes { get; }
    public string PersistedStateKey { get; }
    public bool[] ActualState { get; }
    public OutgoingEventType OutgoingEventType { get; }
}