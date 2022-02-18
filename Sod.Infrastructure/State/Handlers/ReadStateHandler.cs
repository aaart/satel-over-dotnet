using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers
{
    public abstract class ReadStateHandler : LoggingCapability, IHandler
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;

        protected ReadStateHandler(IStore store, IManipulator manipulator)
        {
            _store = store;
            _manipulator = manipulator;
        }

        public async Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters)
        {
            var (status, satelState) = await ManipulatorMethod(_manipulator);
            ValidateStatus(status);

            var persistedState = await _store.GetAsync<bool[]>(PersistedStateKey);
            ValidateState(persistedState, satelState);

            var changes = new Dictionary<string, object>();
            for (int i = 0; i < persistedState.Length; i++)
            {
                if (persistedState[i] != satelState[i])
                {
                    changes.Add($"{Convert.ToString(i + 1)}", satelState[i]);
                }
            }

            if (changes.Any())
            {
                return new[]
                {
                    new SatelTask(TaskType.UpdateStorage, new Dictionary<string, object> { { PersistedStateKey, satelState } }),
                    new SatelTask(NotificationTaskType, changes)
                };
            }
            
            return Enumerable.Empty<SatelTask>();
        }

        protected abstract string PersistedStateKey { get; }

        protected abstract TaskType NotificationTaskType { get; }
        
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