using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateGlobalBroadcastTask : SatelTask
{
    public ActualStateGlobalBroadcastTask(string invokedTimestamp)
    {
        InvokedTimestamp = invokedTimestamp;
        OutgoingEventType = OutgoingEventType.GlobalStateBroadcast;
        OutputCount = 0;
    }

    public string InvokedTimestamp { get; }
    public OutgoingEventType OutgoingEventType { get; }
    public int OutputCount { get; }
}
