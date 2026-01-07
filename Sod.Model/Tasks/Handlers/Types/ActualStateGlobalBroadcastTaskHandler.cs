using Sod.Infrastructure.Satel.Communication;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateGlobalBroadcastTaskHandler : BaseHandler<ActualStateGlobalBroadcastTask>
{
    private readonly IManipulator _manipulator;

    public ActualStateGlobalBroadcastTaskHandler(IManipulator manipulator)
    {
        _manipulator = manipulator;
    }

    protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateGlobalBroadcastTask data)
    {
        var (_, inputs) = await _manipulator.ReadInputs();
        var (_, outputs) = await _manipulator.ReadOutputs();
        var (_, armedPartitions) = await _manipulator.ReadArmedPartitions();
        var (_, suppressedPartitions) = await _manipulator.ReadSuppressedPartitions();
        var (_, alarmTriggered) = await _manipulator.ReadAlarmTriggered();

        var payload = new
        {
            inputs,
            outputs,
            armedPartitions,
            suppressedPartitions,
            alarmTriggered,
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            invokedTimestamp = data.InvokedTimestamp
        };

        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

        return new[] { new GlobalStateNotificationTask(jsonPayload, OutgoingEventType.GlobalStateBroadcast) };
    }
}
