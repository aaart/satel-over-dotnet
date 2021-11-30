using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;
using Sod.Tests.Infrastructure.State.Loop.ReadStateTestsHelpers;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Sod.Tests.Infrastructure.State.Loop
{
    public class ReadStateTests
    {
        private readonly Mock<IStore> _storeMock = new Mock<IStore>();
        private readonly Mock<IManipulator> _manipulatorMock = new Mock<IManipulator>();
        private readonly Mock<IOutgoingChangeNotifier> _eventPublisherMock = new Mock<IOutgoingChangeNotifier>();

        [Theory]
        [InlineData(CommandStatus.InvalidCrc)]
        [InlineData(CommandStatus.InvalidFrame)]
        [InlineData(CommandStatus.NotSent)]
        [InlineData(CommandStatus.InvalidCommandReceived)]
        [InlineData(CommandStatus.NotSupportedCommand)]
        public async Task GivenStateReadStep_WhenManipulatorResultDoesNotIndicateSuccess_ExpectExceptionThrown(CommandStatus commandStatus)
        {
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((commandStatus, Array.Empty<bool>())),
                _ => { });

            await Awaiting(async () => await testReadState.ExecuteAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GivenStateReadStep_WhenReturnedStateLengthDiffersFromPersistedStateLength_ExpectExceptionThrown()
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new bool[1]));
            
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((CommandStatus.Processed, new bool[2])),
                _ => { });

            await Awaiting(async () => await testReadState.ExecuteAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [InlineData(new bool[0])]
        [InlineData(new [] { false })]
        public async Task GivenStateReadStep_WhenReturnedStateLengthIsSameAsPersistedSame_ExpectNoChangeNotification(bool[] state)
        {
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(state));
            
            var notified = false;
            
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((CommandStatus.Processed, state)),
                _ => notified = true);

            await testReadState.ExecuteAsync();
            notified.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(new bool[0])]
        [InlineData(new [] { false })]
        public async Task GivenStateReadStep_WhenReturnedStateLengthIsSameAsPersistedSame_ExpectNoStoreUpdate(bool[] state)
        {
            var storeUpdated = false;
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(state));
            _storeMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(() => Task.Run(() => storeUpdated = true));
            
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((CommandStatus.Processed, state)),
                _ => { });

            await testReadState.ExecuteAsync();
            storeUpdated.Should().BeFalse();
        }
        
        [Fact]
        public async Task GivenStateReadStep_WhenReturnedStateLengthIsSameAsPersistedSame_ExpectNotification()
        {
            var notified = false;
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new [] { false }));
            
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((CommandStatus.Processed, new [] { true })),
                _ => notified = true);

            await testReadState.ExecuteAsync();
            notified.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenStateReadStep_WhenReturnedStateLengthIsSameAsPersistedSame_ExpectStoreUpdated()
        {
            var storeUpdated = false;
            _storeMock.Setup(x => x.GetAsync<bool[]>(It.IsAny<string>())).Returns(Task.FromResult(new [] { false }));
            _storeMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(() => Task.Run(() => storeUpdated = true));
            
            var testReadState = new TestReadState(
                _storeMock.Object, 
                _manipulatorMock.Object, 
                _eventPublisherMock.Object,
                () => Task.FromResult((CommandStatus.Processed, new [] { true })),
                _ => { });

            await testReadState.ExecuteAsync();
            storeUpdated.Should().BeTrue();
        }
    }
}