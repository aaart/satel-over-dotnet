using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;
using Sod.Model.Tools;

namespace Sod.Model.Tasks.Handlers.Types
{
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
                case IOBinaryReadManipulatorMethod.Inputs:
                case IOBinaryReadManipulatorMethod.Outputs:
                    return new[] { new ActualStateBinaryIOPostReadTask(changes, data.PersistedStateKey, actualState, data.OutgoingEventType) };
                case IOBinaryReadManipulatorMethod.ArmedPartitions:
                case IOBinaryReadManipulatorMethod.AlarmTriggered:
                    return new[] { new ActualStateAlarmIOPostReadTask(changes, data.PersistedStateKey, actualState, data.OutgoingEventType) };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<(CommandStatus, bool[])> ManipulatorMethod(IOBinaryReadManipulatorMethod method)
        {
            switch (method)
            {
                case IOBinaryReadManipulatorMethod.Inputs:
                    return await _manipulator.ReadInputs();
                case IOBinaryReadManipulatorMethod.Outputs:
                    return await _manipulator.ReadOutputs();
                case IOBinaryReadManipulatorMethod.ArmedPartitions:
                    return await _manipulator.ReadArmedPartitions();
                case IOBinaryReadManipulatorMethod.AlarmTriggered:
                    return await _manipulator.ReadAlarmTriggered();
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        private void ValidateStatus(CommandStatus status)
        {
            if (status != CommandStatus.Processed)
            {
                throw new InvalidOperationException($"{status} status cannot be processed.");
            }
        }

        private void ValidateState(bool[] persistedState, bool[] satelState)
        {
            if (persistedState.Length != satelState.Length)
            {
                throw new InvalidOperationException($"Current satel state and persisted state have different lengths!");
            }
        }
    }
}