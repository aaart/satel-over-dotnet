using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public class ReadOutputs : ReadState
    {
        public ReadOutputs(IStore store, IManipulator manipulator, IOutgoingChangeNotifier outgoingChangeNotifier) 
            : base(store, manipulator, outgoingChangeNotifier)
        {
            
        }

        protected override string PersistedStateKey => Constants.Store.OutputsStateKey;
        
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => Manipulator.ReadOutputs();

        protected override void Notify(IEnumerable<(int reference, bool value)> changedStates)
        {
            foreach (var changedState in changedStates)
            {
                OutgoingChangeNotifier.Publish(new OutgoingChange(EventType.OutputsStateChanged, changedState.reference, changedState.value.ToString()));
            }
        }
    }
}