using Sod.Model.Tasks;

namespace Sod.Model.Events.Incoming;

public interface ISatelTaskFactory
{
    IncomingEventType Type { get; }
    BaseSatelTask CreateTask(string payload, int ioIndex, bool notify);
}
