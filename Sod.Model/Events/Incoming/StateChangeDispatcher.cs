using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Events.Incoming;

public class StateChangeDispatcher : LoggingCapability, IStateChangeDispatcher
{
    private readonly IncomingEventType _incomingEventType;
    private readonly int _ioIndex;
    private readonly bool _notify;
    private readonly ITaskQueue _queue;

    public StateChangeDispatcher(IncomingEventType incomingEventType, int ioIndex, bool notify, ITaskQueue queue)
    {
        _incomingEventType = incomingEventType;
        _ioIndex = ioIndex;
        _notify = notify;
        _queue = queue;
    }

    public async Task HandleAsync(string payload)
    {
        Logger.LogInformation($"Event received for IOIndex = {_ioIndex} and event type = {_incomingEventType.ToString()}. Payload: {payload}");
        if (_incomingEventType == IncomingEventType.BinaryOutput)
        {
            var task = new ActualStateBinaryIOUpdateTask(
                new List<IOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(payload) } },
                _notify,
                OutgoingEventType.OutputsStateChanged);
            await _queue.EnqueueAsync(task);
        }
        else if (_incomingEventType == IncomingEventType.BinaryOutput)
        {
            // TODO: Implement this!
            throw new NotImplementedException("Action still not implemented");
;        }
        else
        {
            throw new InvalidOperationException($"Cannot process incoming event. Event type = {_incomingEventType}, Index = {_ioIndex}");
        }
    }
}