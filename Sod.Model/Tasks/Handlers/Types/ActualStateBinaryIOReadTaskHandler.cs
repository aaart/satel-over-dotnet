using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;
using Sod.Model.Tools;

namespace Sod.Model.Tasks.Handlers.Types;

public class ActualStateBinaryIOReadTaskHandler : BaseHandler<ActualStateBinaryIOReadTask>
{
    private readonly IStore _store;
    private readonly IManipulator _manipulator;

    public ActualStateBinaryIOReadTaskHandler(IStore store, IManipulator manipulator)
    {
        _store = store;
        _manipulator = manipulator;
    }

    protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateBinaryIOReadTask data)
    {
        var (status, actualState) = await ManipulatorMethod(data.Method);
        ValidateStatus(status);

        var persistedState = await _store.GetAsync<bool[]>(data.PersistedStateKey);
        ValidateState(persistedState, actualState);

        var changes = IO.ExtractIOChanges(persistedState, actualState);

        switch (data.Method)
        {
            case IOBinaryReadType.Inputs:
            case IOBinaryReadType.Outputs:
            case IOBinaryReadType.SuppressedPartitions:
                return [new ActualStateBinaryIOPostReadTask(changes, data.PersistedStateKey, actualState, data.OutgoingEventType)];
            case IOBinaryReadType.ArmedPartitions:
            case IOBinaryReadType.AlarmTriggered:
                return [new ActualStateAlarmIOPostReadTask(changes, data.PersistedStateKey, actualState, data.OutgoingEventType)];
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task<(CommandStatus, bool[])> ManipulatorMethod(IOBinaryReadType method)
    {
        switch (method)
        {
            case IOBinaryReadType.Inputs:
                return await _manipulator.ReadInputs();
            case IOBinaryReadType.Outputs:
                return await _manipulator.ReadOutputs();
            case IOBinaryReadType.ArmedPartitions:
                return await _manipulator.ReadArmedPartitions();
            case IOBinaryReadType.AlarmTriggered:
                return await _manipulator.ReadAlarmTriggered();
            case IOBinaryReadType.SuppressedPartitions:
                return await _manipulator.ReadSuppressedPartitions();
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }

    private void ValidateStatus(CommandStatus status)
    {
        if (status != CommandStatus.Processed) throw new InvalidOperationException($"{status} status cannot be processed.");
    }

    private void ValidateState(bool[] persistedState, bool[] satelState)
    {
        if (persistedState.Length != satelState.Length) throw new InvalidOperationException($"Current satel state and persisted state have different lengths!");
    }
}