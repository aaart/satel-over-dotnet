using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Handlers.Types;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Handlers
{
    [TestFixture]
    public class ActualStateGlobalBroadcastTaskHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnGlobalStateNotificationTask_WithCorrectPayload()
        {
            // Arrange
            var manipulatorMock = new Mock<IManipulator>();
            var handler = new ActualStateGlobalBroadcastTaskHandler(manipulatorMock.Object);
            var invokedTimestamp = "123456789";
            var task = new ActualStateGlobalBroadcastTask(invokedTimestamp);

            var inputs = new bool[] { true, false };
            var outputs = new bool[] { false, true };
            var armedPartitions = new bool[] { true, true };
            var suppressedPartitions = new bool[] { false, false };
            var alarmTriggered = new bool[] { true, false };

            manipulatorMock.Setup(m => m.ReadInputs()).ReturnsAsync((CommandStatus.Processed, inputs));
            manipulatorMock.Setup(m => m.ReadOutputs()).ReturnsAsync((CommandStatus.Processed, outputs));
            manipulatorMock.Setup(m => m.ReadArmedPartitions()).ReturnsAsync((CommandStatus.Processed, armedPartitions));
            manipulatorMock.Setup(m => m.ReadSuppressedPartitions()).ReturnsAsync((CommandStatus.Processed, suppressedPartitions));
            manipulatorMock.Setup(m => m.ReadAlarmTriggered()).ReturnsAsync((CommandStatus.Processed, alarmTriggered));

            // Act
            var result = await handler.Handle(task);

            // Assert
            Assert.AreEqual(1, result.Count());
            var resultTask = result.First() as GlobalStateNotificationTask;
            Assert.IsNotNull(resultTask);
            Assert.AreEqual(OutgoingEventType.GlobalStateBroadcast, resultTask.OutgoingEventType);

            // Basic JSON check
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"inputs\":[true,false]"));
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"outputs\":[false,true]"));
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"armedPartitions\":[true,true]"));
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"suppressedPartitions\":[false,false]"));
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"alarmTriggered\":[true,false]"));
            Assert.IsTrue(resultTask.JsonPayload.Contains($"\"invokedTimestamp\":\"{invokedTimestamp}\""));
            Assert.IsTrue(resultTask.JsonPayload.Contains("\"timestamp\":"));
        }
    }
}
