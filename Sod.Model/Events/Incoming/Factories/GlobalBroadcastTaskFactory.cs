using Sod.Model.Tasks;

namespace Sod.Model.Events.Incoming.Factories;

public class GlobalBroadcastTaskFactory : ISatelTaskFactory
{
    public IncomingEventType Type => IncomingEventType.GlobalBroadcast;

    public BaseSatelTask CreateTask(string payload, int ioIndex, bool notify)
    {
        return new ActualStateGlobalBroadcastTask(payload);
    }
}
