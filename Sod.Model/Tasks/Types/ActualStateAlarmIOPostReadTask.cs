using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateAlarmIOPostReadTask : SatelTask
{
    public ActualStateAlarmIOPostReadTask(IList<IOState> changes, string persistedStateKey, bool[] actualState, OutgoingEventType outgoingEventType)
    {
        Changes = changes;
        PersistedStateKey = persistedStateKey;
        ActualState = actualState;
        OutgoingEventType = outgoingEventType;
    }

    public IList<IOState> Changes { get; }
    public string PersistedStateKey { get; } 
    public bool[] ActualState { get; } 
    public OutgoingEventType OutgoingEventType { get; }
}