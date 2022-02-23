using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Sod.Infrastructure.Configuration;
using Sod.Infrastructure.Satel.Communication;
using Sod.Tests.Infrastructure.Satel.Mocks;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Sod.Tests.Infrastructure.Satel
{
    public class ManipulatorTests
    {
        [Fact]
        public void GivenNullUserCode_WhenCommunicationInterfaceIsValid_ExpectException()
        {
            var socketSender = new MockSocketSender(frameToSend => Task.FromResult(1));
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((0, new ArraySegment<byte>())));

            Invoking(() => new Manipulator(new GenericCommunicationInterface(socketReceiver, socketSender), new OptionsWrapper<SatelUserCodeOptions>(new SatelUserCodeOptions())))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void GivenEmptyUserCode_WhenCommunicationInterfaceIsValid_ExpectException()
        {
            var socketSender = new MockSocketSender(frameToSend => Task.FromResult(1));
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((0, new ArraySegment<byte>())));

            Invoking(() => new Manipulator(new GenericCommunicationInterface(socketReceiver, socketSender), new OptionsWrapper<SatelUserCodeOptions>(new SatelUserCodeOptions { UserCode = "" })))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}