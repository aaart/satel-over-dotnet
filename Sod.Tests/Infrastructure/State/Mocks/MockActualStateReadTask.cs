using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Mocks
{
    public class MockActualStateReadTask : ActualStateReadTask
    {
        public MockActualStateReadTask() 
            : base(string.Empty, NotificationTaskType.NotifyIOChanged, IOReadManipulatorMethod.Inputs, OutgoingEventType.InputsStateChanged)
        {
        }
    }
}