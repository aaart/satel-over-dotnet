using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Mocks
{
    public class MockReadStateTask : ReadStateTask
    {
        public MockReadStateTask() 
            : base(string.Empty, NotificationTaskType.NotifyIOChanged, IOReadManipulatorMethod.Inputs, OutgoingEventType.InputsStateChanged)
        {
        }
    }
}