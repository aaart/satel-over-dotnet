using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.DataStructures;
using Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers;
using Sod.Tests.Infrastructure.State.Mocks;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Sod.Tests.Infrastructure.State.Handlers;

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

        var testReadStateHandler = new TestActualStateBinaryIOReadActualIOStateBinaryIOTaskHandler(
            _storeMock.Object,
            _manipulatorMock.Object);

        await Awaiting(async () => await testReadStateHandler.Handle(new MockActualStateBinaryIOReadTask()))
            .Should()
            .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GivenReadStateHandler_WhenReturnedStateLengthDiffersFromPersistedStateLength_ExpectExceptionThrown()
    {
        _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[1]));
        _manipulatorMock.Setup(x => x.ReadInputs()).Returns(() => Task.FromResult((CommandStatus.Processed, new bool[2])));

        var testReadStateHandler = new TestActualStateBinaryIOReadActualIOStateBinaryIOTaskHandler(
            _storeMock.Object,
            _manipulatorMock.Object);

        await Awaiting(async () => await testReadStateHandler.Handle(new MockActualStateBinaryIOReadTask()))
            .Should()
            .ThrowAsync<InvalidOperationException>();
    }


    public static IEnumerable<object[]> CreateNotSuccessfulStatuses()
    {
        return Enum.GetNames<CommandStatus>()
            .Where(x => x != Enum.GetName(CommandStatus.Processed))
            .Select(x => new object[] { x });
    }
}