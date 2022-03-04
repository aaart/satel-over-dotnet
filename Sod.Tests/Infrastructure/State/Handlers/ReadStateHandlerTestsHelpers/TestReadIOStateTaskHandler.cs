using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.State.Tasks.Handlers.IOStateRead;
using Sod.Infrastructure.Storage;

namespace Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers
{
    public class TestReadIOStateTaskHandler : ReadIOStateTaskHandler
    {

        public TestReadIOStateTaskHandler(
            IStore store, 
            IManipulator manipulator) 
            : base(store, manipulator)
        {
        }
    }
}