using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public abstract class ReadStateHandler : LoggingCapability, IHandler
    {
        private readonly IStore _store;
        private readonly ITaskQueue _queue;
        private readonly IManipulator _manipulator;

        protected ReadStateHandler(IStore store, ITaskQueue queue, IManipulator manipulator)
        {
            _store = store;
            _queue = queue;
            _manipulator = manipulator;
        }

        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            var (status, satelState) = await ManipulatorMethod(_manipulator);
            ValidateStatus(status);

            var persistedState = await _store.GetAsync<bool[]>(PersistedStateKey);
            ValidateState(persistedState, satelState);

            var stateUpdated = false;
            for (int i = 0; i < persistedState.Length; i++)
            {
                if (persistedState[i] != satelState[i])
                {
                    persistedState[i] = satelState[i];
                    stateUpdated = true;
                }
            }

            if (stateUpdated)
            {
                await _queue.Enqueue(new SatelTask(TaskType.UpdateStorage, new Dictionary<string, object> { { PersistedStateKey, persistedState } }));
            }
        }

        protected abstract string PersistedStateKey { get; }

        protected abstract Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator);

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