using Sod.Infrastructure.Satel.Communication;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Handlers.Policies;
using Sod.Model.Tasks.Handlers.Types;

namespace Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers
{
    public class TestActualStateBinaryIOReadActualIOStateBinaryIOTaskHandler : ActualStateBinaryIOReadTaskHandler
    {

        public TestActualStateBinaryIOReadActualIOStateBinaryIOTaskHandler(
            IStore store, 
            IManipulator manipulator,
            IPostReadPolicy postReadPolicy) 
            : base(store, manipulator, postReadPolicy)
        {
        }
    }
}