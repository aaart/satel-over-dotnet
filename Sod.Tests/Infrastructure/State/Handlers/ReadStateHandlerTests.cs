using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;
using Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers;
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
            var testReadStateHandler = new TestReadStateStateHandler(
                _storeMock.Object,
                _manipulatorMock.Object,
                _ => Task.FromResult((commandStatus, Array.Empty<bool>())));

            await Awaiting(async () => await testReadStateHandler.Handle(new Dictionary<string, object>()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenReturnedStateLengthDiffersFromPersistedStateLength_ExpectExceptionThrown()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[1]));

            var testReadStateHandler = new TestReadStateStateHandler(
                _storeMock.Object,
                _manipulatorMock.Object,
                _ => Task.FromResult((CommandStatus.Processed, new bool[2])));

            await Awaiting(async () => await testReadStateHandler.Handle(new Dictionary<string, object>()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenSatelStateIsTheSameAsPersistedState_ExpectNoTasksReturned()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[128]));

            var testReadStateHandler = new TestReadStateStateHandler(
                _storeMock.Object,
                _manipulatorMock.Object,
                _ => Task.FromResult((CommandStatus.Processed, new bool[128])));

            (await testReadStateHandler.Handle(new Dictionary<string, object>()))
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task GivenReadStateHandler_WhenSatelStateDiffersFromPersistedState_ExpectTasksReturned()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[128]));

            var testReadStateHandler = new TestReadStateStateHandler(
                _storeMock.Object,
                _manipulatorMock.Object,
                _ =>
                {
                    var state = new bool[128];
                    state[0] = true;
                    return Task.FromResult((CommandStatus.Processed, state));
                });

            var tasks = (await testReadStateHandler.Handle(new Dictionary<string, object>())).ToArray();
            tasks
                .Should()
                .HaveCount(2);

            tasks[0].Type.Should().Be(TaskType.UpdateStorage);
            tasks[1].Type.Should().Be(TaskType.NotifyInputsChanged);
        }

        public static IEnumerable<object[]> CreateNotSuccessfulStatuses() => 
            Enum.GetNames<CommandStatus>()
                .Where(x => x != Enum.GetName(CommandStatus.Processed))
                .Select(x => new object[] { x });
    }
}