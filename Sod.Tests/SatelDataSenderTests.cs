using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sod.Core;
using Sod.Tests.Mocks;
using Xunit;
using static Sod.Core.Communication;

namespace Sod.Tests
{
    public class SatelDataSenderTests
    {
        [Fact]
        public async void GivenStateArray_WhenAllSuccess_ThenTrue()
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(7));
            var success = await SendAsync(socketSender,Command.NewData, Array.Empty<byte>(), Array.Empty<byte>());
            success.Should().BeTrue();
        }
        
        [Fact]
        public async void GivenStateArray_WhenNotAllSuccess_ThenFalse()
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(22));
            var success = await SendAsync(socketSender, Command.NewData, Array.Empty<byte>(), Array.Empty<byte>());
            success.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(Command.OutputsSwitch)]
        [InlineData(Command.ArmInMode0)]
        public void GivenUpdateCommand_WhenNoUserCodeProvided_ThenException(Command cmd)
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(22));
        
            new Func<Task<bool>>(async () => await SendAsync(socketSender, cmd, new byte[16], Array.Empty<byte>()))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}