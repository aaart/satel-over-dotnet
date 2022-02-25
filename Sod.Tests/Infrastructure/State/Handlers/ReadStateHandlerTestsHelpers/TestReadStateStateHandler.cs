using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.State.Tasks.Handlers.StateRead;
using Sod.Infrastructure.Storage;

namespace Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers
{
    public class TestReadStateStateHandler : ReadStateStateHandler
    {
        private readonly Func<IManipulator, Task<(CommandStatus, bool[])>> _manipulatorMethodImpl;

        public TestReadStateStateHandler(
            IStore store, 
            IManipulator manipulator,
            Func<IManipulator, Task<(CommandStatus, bool[])>> manipulatorMethodImpl) 
            : base(store, manipulator)
        {
            _manipulatorMethodImpl = manipulatorMethodImpl;
        }
        
        protected override string PersistedStateKey => string.Empty;
        protected override TaskType NotificationTaskType => TaskType.NotifyInputsChanged;

        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => _manipulatorMethodImpl(manipulator);


    }
}