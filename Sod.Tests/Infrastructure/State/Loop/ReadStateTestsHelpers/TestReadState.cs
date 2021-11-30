using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.State.Loop.StepType;
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
            IOutgoingChangeNotifier outgoingChangeNotifier,
            Func<Task<(CommandStatus, bool[])>> manipulatorMethodImpl,
            Action<IEnumerable<(int reference, bool value)>> notifyMethodImpl) 
            : base(store, manipulator, outgoingChangeNotifier)
        {
            _manipulatorMethodImpl = manipulatorMethodImpl;
            _notifyMethodImpl = notifyMethodImpl;
        }
        
        protected override string PersistedStateKey => string.Empty;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod() => _manipulatorMethodImpl();

        protected override void Notify(IEnumerable<(int reference, bool value)> changedStates) => _notifyMethodImpl(changedStates);

    }
}