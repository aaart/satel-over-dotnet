using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;

namespace Sod.Model.Tasks.Handlers.Impl;

public class ActualStateBinaryIOUpdateTaskHandler(IManipulator manipulator) : BaseHandler<ActualStateBinaryIOUpdateTask>
{
    protected override async Task<IEnumerable<BaseSatelTask>> Handle(ActualStateBinaryIOUpdateTask data)
    {
        Logger.LogDebug($"{nameof(ActualStateBinaryIOUpdateTaskHandler)} is executing.");
        var disableOutputs = new bool[data.OutputCount];
        var enableOutputs = new bool[data.OutputCount];
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

        var tasks = new List<BaseSatelTask>();
        if (anyEnabled)
            switch (data.Method)
            {
                case IOBinaryUpdateType.Outputs:
                    await manipulator.EnableOutputs(enableOutputs);
                    break;
                case IOBinaryUpdateType.Partitions:
                    await manipulator.ArmInMode0(enableOutputs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        if (anyDisabled)
            switch (data.Method)
            {
                case IOBinaryUpdateType.Outputs:
                    await manipulator.DisableOutputs(disableOutputs);
                    break;
                case IOBinaryUpdateType.Partitions:
                    await manipulator.DisArm(disableOutputs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        if (notifications.Any() && data.NotifyChanged) tasks.Add(new ActualStateChangedNotificationTask(notifications, data.EventType));

        return tasks;
    }
}