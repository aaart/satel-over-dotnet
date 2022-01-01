using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Events.Outgoing;
using Sod.Infrastructure.Satel.State.Loop.StepType;
using Sod.Infrastructure.Store;

namespace Sod.Tests.Infrastructure.State.Loop.ReadStateTestsHelpers
{
    public class TestReadState : ReadState
    {
        private readonly Func<Task<(CommandStatus, bool[])>> _manipulatorMethodImpl;
        private readonly Action<IEnumerable<(int reference, bool value)>> _notifyMethodImpl;

        public TestReadState(
            IStore store, 
            IManipulator manipulator, 
            IOutgoingEventPublisher outgoingEventPublisher,
            Func<Task<(CommandStatus, bool[])>> manipulatorMethodImpl,
            Action<IEnumerable<(int reference, bool value)>> notifyMethodImpl) 
            : base(store, manipulator, outgoingEventPublisher)
        {
            _manipulatorMethodImpl = manipulatorMethodImpl;
            _notifyMethodImpl = notifyMethodImpl;
        }
        
        protected override string PersistedStateKey => string.Empty;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => _manipulatorMethodImpl();

        protected override void Notify(IEnumerable<(int reference, bool value)> changedStates) => _notifyMethodImpl(changedStates);

    }
}