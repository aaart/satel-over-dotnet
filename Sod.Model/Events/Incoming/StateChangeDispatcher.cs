using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;
using Sod.Model.Tasks;

namespace Sod.Model.Events.Incoming;

public class StateChangeDispatcher : LoggingCapability, IStateChangeDispatcher
{
    private readonly ISatelTaskFactory _taskFactory;
    private readonly int _ioIndex;
    private readonly bool _notify;
    private readonly ITaskQueue _queue;

    public StateChangeDispatcher(ISatelTaskFactory taskFactory, int ioIndex, bool notify, ITaskQueue queue)
    {
        _taskFactory = taskFactory;
        _ioIndex = ioIndex;
        _notify = notify;
        _queue = queue;
    }

    public async Task HandleAsync(string payload)
    {
        Logger.LogInformation($"Event received for IOIndex = {_ioIndex} and event type = {_taskFactory.Type.ToString()}. Payload: {payload}");

        var task = _taskFactory.CreateTask(payload, _ioIndex, _notify);

        await _queue.EnqueueAsync(task);
    }
}
