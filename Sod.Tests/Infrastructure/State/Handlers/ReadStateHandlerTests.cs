using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.State.Tasks;
using Sod.Infrastructure.State.Tasks.Handlers.Notifications;
using Sod.Infrastructure.State.Tasks.Handlers.StorageUpdate;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes;
using Sod.Infrastructure.Storage.TaskTypes.StorageUpdate;
using Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers;
using Sod.Tests.Infrastructure.State.Mocks;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Sod.Tests.Infrastructure.State.Handlers
{
    public class ReadStateHandlerTests
    {
        private readonly Mock<IStore> _storeMock = new();
        private readonly Mock<IManipulator> _manipulatorMock = new();

        [Theory]
        [MemberData(nameof(CreateNotSuccessfulStatuses))]
        public async Task GivenReadStateHandler_WhenManipulatorResultDoesNotIndicateSuccess_ExpectExceptionThrown(string commandStatusName)
        {
            var commandStatus = Enum.Parse<CommandStatus>(commandStatusName);
            _manipulatorMock.Setup(x => x.ReadInputs()).Returns(() => Task.FromResult((commandStatus, Array.Empty<bool>())));
            
            var testReadStateHandler = new TestReadIOStateTaskHandler(
                _storeMock.Object,
                _manipulatorMock.Object);

            await Awaiting(async () => await testReadStateHandler.Handle(new MockReadStateTask()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenReturnedStateLengthDiffersFromPersistedStateLength_ExpectExceptionThrown()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[1]));
            _manipulatorMock.Setup(x => x.ReadInputs()).Returns(() => Task.FromResult((CommandStatus.Processed, new bool[2])));
            
            var testReadStateHandler = new TestReadIOStateTaskHandler(
                _storeMock.Object,
                _manipulatorMock.Object);

            await Awaiting(async () => await testReadStateHandler.Handle(new MockReadStateTask()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenSatelStateIsTheSameAsPersistedState_ExpectNoTasksReturned()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[128]));
            _manipulatorMock.Setup(x => x.ReadInputs()).Returns(() => Task.FromResult((CommandStatus.Processed, new bool[128])));
            
            var testReadStateHandler = new TestReadIOStateTaskHandler(
                _storeMock.Object,
                _manipulatorMock.Object);

            (await testReadStateHandler.Handle(new MockReadStateTask()))
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenSatelStateDiffersFromPersistedState_ExpectTasksReturned()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[128]));
            _manipulatorMock.Setup(x => x.ReadInputs()).Returns(() =>
            {
                var state = new bool[128];
                state[0] = true;
                return Task.FromResult((CommandStatus.Processed, state));
            });
            
            var testReadStateHandler = new TestReadIOStateTaskHandler(
                _storeMock.Object,
                _manipulatorMock.Object);

            var tasks = (await testReadStateHandler.Handle(new MockReadStateTask())).ToArray();
            tasks
                .Should()
                .HaveCount(2);

            tasks[0].GetType().Should().Be(typeof(StorageUpdateTask));
            tasks[1].GetType().Should().Be(typeof(ChangeNotificationTaskHandler));
        }

        public static IEnumerable<object[]> CreateNotSuccessfulStatuses() =>
            Enum.GetNames<CommandStatus>()
                .Where(x => x != Enum.GetName(CommandStatus.Processed))
                .Select(x => new object[] { x });
    }
}