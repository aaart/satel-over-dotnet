using Sod.Model.CommonTypes;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Mocks
{
    public class MockReadStateTask : ReadStateTask
    {
        public MockReadStateTask() 
            : base(string.Empty, TaskType.NotifyIOChanged, IOReadManipulatorMethod.Inputs, OutgoingEventType.InputsStateChanged)
        {
        }
    }
}