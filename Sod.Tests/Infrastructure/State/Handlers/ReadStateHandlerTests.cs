﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.Storage;
using Sod.Tests.Infrastructure.State.Handlers.ReadStateHandlerTestsHelpers;
using Xunit;
using static FluentAssertions.FluentActions;
using Task = System.Threading.Tasks.Task;

namespace Sod.Tests.Infrastructure.State.Handlers
{
    public class ReadStateHandlerTests
    {
        private readonly Mock<IStore> _storeMock = new();
        private readonly Mock<IManipulator> _manipulatorMock = new();
        private readonly Mock<IOutgoingEventPublisher> _eventPublisherMock = new();

        [Theory]
        [InlineData(CommandStatus.InvalidCrc)]
        [InlineData(CommandStatus.InvalidFrame)]
        [InlineData(CommandStatus.NotSent)]
        [InlineData(CommandStatus.InvalidCommandReceived)]
        [InlineData(CommandStatus.NotSupportedCommand)]
        public async Task GivenStateReadStep_WhenManipulatorResultDoesNotIndicateSuccess_ExpectExceptionThrown(CommandStatus commandStatus)
        {
            var testReadState = new TestReadStateHandler(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _ => Task.FromResult((commandStatus, Array.Empty<bool>())));

            await Awaiting(async () => await testReadState.Handle(new Dictionary<string, object>()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenStateReadStep_WhenReturnedStateLengthDiffersFromPersistedStateLength_ExpectExceptionThrown()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[1]));
            
            var testReadState = new TestReadStateHandler(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _ => Task.FromResult((CommandStatus.Processed, new bool[2])));

            await Awaiting(async () => await testReadState.Handle(new Dictionary<string, object>()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}