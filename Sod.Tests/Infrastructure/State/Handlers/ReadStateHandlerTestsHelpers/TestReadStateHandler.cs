using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.State.Handlers;
using Sod.Infrastructure.Storage;

namespace Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers
{
    public class TestReadStateHandler : ReadStateHandler
    {
        private readonly Func<IManipulator, Task<(CommandStatus, bool[])>> _manipulatorMethodImpl;

        public TestReadStateHandler(
            IStore store, 
            IManipulator manipulator,
            Func<IManipulator, Task<(CommandStatus, bool[])>> manipulatorMethodImpl) 
            : base(store, manipulator)
        {
            _manipulatorMethodImpl = manipulatorMethodImpl;
        }
        
        protected override string PersistedStateKey => string.Empty;
        protected override TaskType NotificationTaskType => TaskType.ReadInputs;

        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => _manipulatorMethodImpl(manipulator);


    }
}