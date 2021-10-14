using System.Threading.Tasks;
using FluentAssertions;
using Sod.Core;
using Sod.Tests.Mocks;
using Xunit;

namespace Sod.Tests
{
    public class SatelDataSenderTests
    {
        [Fact]
        public async void GivenStateArray_WhenAllSuccess_ThenTrue()
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(23));
            var satelDataSender = new SatelDataSender(socketSender);
            var success = await satelDataSender.SendAsync(Command.NewData, new bool [128]);
            success.Should().BeTrue();
        }
        
        [Fact]
        public async void GivenStateArray_WhenNotAllSuccess_ThenFalse()
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(22));
            var satelDataSender = new SatelDataSender(socketSender);
            var success = await satelDataSender.SendAsync(Command.NewData, new bool [128]);
            success.Should().BeFalse();
        }
    }
}