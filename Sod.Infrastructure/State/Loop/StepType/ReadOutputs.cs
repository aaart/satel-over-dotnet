using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public class ReadOutputs : ReadState
    {
        public ReadOutputs(IStore store, IManipulator manipulator, IEventPublisher eventPublisher) 
            : base(store, manipulator, eventPublisher)
        {
            
        }

        protected override string PersistedStateKey => Constants.Store.OutputsStateKey;
        
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => Manipulator.ReadOutputs();

        protected override void Notify(IEnumerable<int> changedStates) => EventPublisher.Publish(new Event(EventType.OutputsStateChanged, changedStates));
    }
}