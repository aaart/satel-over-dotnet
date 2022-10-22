using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Mocks
{
    public class MockActualStateBinaryIOReadTask : ActualStateBinaryIOReadTask
    {
        public MockActualStateBinaryIOReadTask() 
            : base(string.Empty, NotificationTaskType.NotifyIOChanged, IOBinaryReadManipulatorMethod.Inputs, OutgoingEventType.InputsStateChanged)
        {
        }
    }
}