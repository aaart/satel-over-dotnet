using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks;

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

        BaseSatelTask task;

        switch (_incomingEventType)
        {
            case IncomingEventType.BinaryOutput:
                task = new ActualStateBinaryIOUpdateTask(
                    new List<BinaryIOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(payload) } },
                    IOBinaryUpdateType.Outputs,
                    _notify,
                    OutgoingEventType.OutputsStateChanged,
                    128);
                break;
            case IncomingEventType.ArmPartition:
                task = new ActualStateBinaryIOUpdateTask(
                    new List<BinaryIOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(payload) } },
                    IOBinaryUpdateType.Partitions,
                    _notify,
                    OutgoingEventType.ArmedPartitionsStateChanged,
                    32);
                break;
            case IncomingEventType.GlobalBroadcast:
                task = new ActualStateGlobalBroadcastTask(payload);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        await _queue.EnqueueAsync(task);
    }
}
