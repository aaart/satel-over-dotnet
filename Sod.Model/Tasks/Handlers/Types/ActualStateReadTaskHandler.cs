using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class ActualStateReadTaskHandler : BaseHandler<ActualStateReadTask>
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;

        public ActualStateReadTaskHandler(IStore store, IManipulator manipulator)
        {
            _store = store;
            _manipulator = manipulator;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateReadTask data)
        {
            var (status, satelState) = await ManipulatorMethod(data.Method);
            ValidateStatus(status);

            var persistedState = await _store.GetAsync<bool[]>(data.PersistedStateKey);
            ValidateState(persistedState, satelState);

            var changes = new List<IOState>();
            for (int i = 0; i < persistedState.Length; i++)
            {
                if (persistedState[i] != satelState[i])
                {
                    changes.Add(new IOState { Index = i + 1, Value = satelState[i] });
                }
            }

            if (changes.Any())
            {
                Logger.LogInformation($"{changes.Count} changes were found.");
                var t1 = new PersistaedStateUpdateTask(data.PersistedStateKey, satelState);
                var t2 = new ActualStateChangedNotificationTask(changes, data.OutgoingEventType); 
                return new SatelTask[] { t1, t2 };
            }

            return Enumerable.Empty<SatelTask>();
        }

        private async Task<(CommandStatus, bool[])> ManipulatorMethod(IOReadManipulatorMethod method)
        {
            switch (method)
            {
                case IOReadManipulatorMethod.Inputs:
                    return await _manipulator.ReadInputs();
                case IOReadManipulatorMethod.Outputs:
                    return await _manipulator.ReadOutputs();
                case IOReadManipulatorMethod.ArmedPartitions:
                    return await _manipulator.ReadArmedPartitions();
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