using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class GlobalStateNotificationTask : SatelTask
{
    public GlobalStateNotificationTask(string jsonPayload, OutgoingEventType outgoingEventType)
    {
        JsonPayload = jsonPayload;
        OutgoingEventType = outgoingEventType;
    }

    public string JsonPayload { get; }
    public OutgoingEventType OutgoingEventType { get; }
}
