using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.State.Tasks;
using Sod.Infrastructure.Storage.TaskTypes;
using Sod.Infrastructure.Storage.TaskTypes.IOStateRead;

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