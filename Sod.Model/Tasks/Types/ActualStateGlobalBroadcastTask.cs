using Sod.Model.Tasks;

namespace Sod.Model.Tasks.Types;

public class ActualStateGlobalBroadcastTask : SatelTask
{
    public ActualStateGlobalBroadcastTask(string invokedTimestamp)
    {
        InvokedTimestamp = invokedTimestamp;
    }

    public string InvokedTimestamp { get; }
}
