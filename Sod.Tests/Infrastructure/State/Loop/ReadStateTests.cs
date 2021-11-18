using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;
using Sod.Tests.Infrastructure.State.Loop.ReadStateTestsHelpers;
using Xunit;

namespace Sod.Tests.Infrastructure.State.Loop
{
    public class ReadStateTests
    {
        private IMock<IStore> _storeMock = new Mock<IStore>();
        private IMock<IManipulator> _manipulatorMock = new Mock<IManipulator>();
        private IMock<IEventPublisher> _eventPublisherMock = new Mock<IEventPublisher>();

        [Theory]
        [InlineData(CommandStatus.InvalidCrc)]
        [InlineData(CommandStatus.InvalidFrame)]
        [InlineData(CommandStatus.NotSent)]
        [InlineData(CommandStatus.InvalidCommandReceived)]
        [InlineData(CommandStatus.NotSupportedCommand)]
        public void WhenSatelStatusDoesNotIndicateSuccess_ExpectExceptionThrown(CommandStatus commandStatus)
        {
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((commandStatus, Array.Empty<bool>())),
                changes => { });

            new Func<Task>(async () => await testReadState.ExecuteAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}