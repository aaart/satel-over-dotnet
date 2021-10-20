﻿using System;
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
            var socketSender = new MockSocketSender(data => Task.FromResult(7));
            var satelDataSender = new SatelDataSender(socketSender);
            var success = await satelDataSender.SendAsync(Command.NewData);
            success.Should().BeTrue();
        }
        
        [Fact]
        public async void GivenStateArray_WhenNotAllSuccess_ThenFalse()
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(22));
            var satelDataSender = new SatelDataSender(socketSender);
            var success = await satelDataSender.SendAsync(Command.NewData);
            success.Should().BeFalse();
        }

        [Theory]
        [InlineData(Command.OutputsSwitch)]
        [InlineData(Command.ArmInMode0)]
        public void GivenUpdateCommand_WhenNoUserCodeProvided_ThenException(Command cmd)
        {
            var socketSender = new MockSocketSender(data => Task.FromResult(22));
            var satelDataSender = new SatelDataSender(socketSender);

            new Func<Task<bool>>(async () => await satelDataSender.SendAsync(cmd, new bool[128]))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}