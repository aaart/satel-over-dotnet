using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public abstract class ReadState : BaseStep
    {
        protected ReadState(
            IStore store, 
            IManipulator manipulator, 
            IOutgoingChangeNotifier outgoingChangeNotifier) 
            : base(store, manipulator, outgoingChangeNotifier)
        {
        }

        protected override async Task ExecuteInternalAsync()
        {
            var (status, satelState) = await ManipulatorMethod();
            ValidateStatus(status);
            
            var persistedState = await Store.GetAsync<bool[]>(PersistedStateKey);
            ValidateState(persistedState, satelState);

            var changedStates = new List<(int, bool)>();
            for (int i = 0; i < persistedState.Length; i++)
            {
                var singlePersistedState = persistedState[i];
                var singleSatelState = satelState[i];

                if (singlePersistedState != singleSatelState)
                {
                    changedStates.Add((i, singleSatelState));
                }
            }

            if (changedStates.Any())
            {
                await Store.SetAsync(PersistedStateKey, satelState);
                Notify(changedStates);
            }
        }

        protected abstract string PersistedStateKey { get; }
        
        protected abstract Task<(CommandStatus, bool[])> ManipulatorMethod();

        protected abstract void Notify(IEnumerable<(int reference, bool value)> changedStates);
        
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