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
        IOBinaryUpdateType updateType;
        OutgoingEventType outgoingEventType;
        switch (_incomingEventType)
        {
            case IncomingEventType.BinaryOutput:
                updateType = IOBinaryUpdateType.Outputs;
                outgoingEventType = OutgoingEventType.OutputsStateChanged;
                break;
            case IncomingEventType.AlarmPartition:
                updateType = IOBinaryUpdateType.Partitions;
                outgoingEventType = OutgoingEventType.ArmedPartitionsStateChanged;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var task = new ActualStateBinaryIOUpdateTask(new List<BinaryIOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(payload) } }, updateType, _notify, outgoingEventType);
        await _queue.EnqueueAsync(task);
    }
}