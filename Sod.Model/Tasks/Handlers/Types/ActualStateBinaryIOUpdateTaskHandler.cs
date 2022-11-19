using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateBinaryIOUpdateTaskHandler : BaseHandler<ActualStateBinaryIOUpdateTask>
{
    private readonly IManipulator _manipulator;

    public ActualStateBinaryIOUpdateTaskHandler(IManipulator manipulator)
    {
        Logger.LogDebug($"{nameof(ActualStateBinaryIOUpdateTaskHandler)} is executing.");
        _manipulator = manipulator;
    }


    protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateBinaryIOUpdateTask data)
    {
        var disableOutputs = new bool[128];
        var enableOutputs = new bool[128];
        var notifications = new List<BinaryIOState>();
        var anyEnabled = false;
        var anyDisabled = false;
        foreach (var state in data.Updates)
        {
            var index = state.Index - 1;
            var enable = state.Value;
            if (enable)
            {
                enableOutputs[index] = true;
                anyEnabled = true;
            }
            else
            {
                disableOutputs[index] = true;
                anyDisabled = true;
            }

            notifications.Add(state);
        }

        var tasks = new List<SatelTask>();
        if (anyEnabled)
            switch (data.Method)
            {
                case IOBinaryUpdateType.Outputs:
                    await _manipulator.EnableOutputs(enableOutputs);
                    break;
                case IOBinaryUpdateType.Partitions:
                    await _manipulator.ArmInMode0(enableOutputs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        if (anyDisabled)
            switch (data.Method)
            {
                case IOBinaryUpdateType.Outputs:
                    await _manipulator.DisableOutputs(disableOutputs);
                    break;
                case IOBinaryUpdateType.Partitions:
                    await _manipulator.DisArm(disableOutputs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        if (notifications.Any() && data.NotifyChanged) tasks.Add(new ActualStateChangedNotificationTask(notifications, data.EventType));

        return tasks;
    }
}