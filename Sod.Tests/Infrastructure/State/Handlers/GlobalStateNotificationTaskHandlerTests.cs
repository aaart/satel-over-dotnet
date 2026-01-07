using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Handlers.Types;
using Sod.Model.Tasks.Types;

namespace Sod.Tests.Infrastructure.State.Handlers
{
    [TestFixture]
    public class GlobalStateNotificationTaskHandlerTests
    {
        [Test]
        public async Task Handle_ShouldPublishEvent()
        {
            // Arrange
            var publisherMock = new Mock<IOutgoingEventPublisher>();
            var handler = new GlobalStateNotificationTaskHandler(publisherMock.Object);
            var jsonPayload = "{\"test\":\"data\"}";
            var task = new GlobalStateNotificationTask(jsonPayload, OutgoingEventType.GlobalStateBroadcast);

            // Act
            var result = await handler.Handle(task);

            // Assert
            publisherMock.Verify(p => p.PublishAsync(It.Is<OutgoingEvent>(e =>
                e.Type == OutgoingEventType.GlobalStateBroadcast &&
                e.Reference == 0 &&
                e.Value == jsonPayload)), Times.Once);

            Assert.IsEmpty(result);
        }
    }
}
