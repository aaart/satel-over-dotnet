using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public class ReadInputs : ReadState
    {
        public ReadInputs(IStore store, IManipulator manipulator, IEventPublisher eventPublisher) 
            : base(store, manipulator, eventPublisher)
        {
            
        }

        protected override string PersistedStateKey => Constants.Store.InputsStateKey;

        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => Manipulator.ReadInputs();
        
        protected override void Notify(IEnumerable<int> changedStates) => EventPublisher.Publish(new Event(EventType.InputsStateChanged, changedStates));
    }
}