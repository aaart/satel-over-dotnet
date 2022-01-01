using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Events.Outgoing;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop.StepType
{
    public class ReadOutputs : ReadState
    {
        public ReadOutputs(IStore store, IManipulator manipulator, IOutgoingEventPublisher outgoingEventPublisher) 
            : base(store, manipulator, outgoingEventPublisher)
        {
            
        }

        protected override string PersistedStateKey => Constants.Store.OutputsStateKey;
        
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => Manipulator.ReadOutputs();

        protected override void Notify(IEnumerable<(int reference, bool value)> changedStates)
        {
            foreach (var changedState in changedStates)
            {
                OutgoingEventPublisher.Publish(new OutgoingEvent(OutgoingEventType.OutputsStateChanged, changedState.reference, changedState.value ? "on" : "off"));
            }
        }
    }
}