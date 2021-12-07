using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop.StepType
{
    public class ReadInputs : ReadState
    {
        public ReadInputs(IStore store, IManipulator manipulator, IOutgoingChangeNotifier outgoingChangeNotifier) 
            : base(store, manipulator, outgoingChangeNotifier)
        {
        }

        protected override string PersistedStateKey => Constants.Store.InputsStateKey;

        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => Manipulator.ReadInputs();
        
        protected override void Notify(IEnumerable<(int reference, bool value)> changedStates)
        {
            foreach (var changedState in changedStates)
            {
                OutgoingChangeNotifier.Publish(new OutgoingChange(EventType.InputsStateChanged, changedState.reference, changedState.value.ToString()));
            }
        }
    }
}