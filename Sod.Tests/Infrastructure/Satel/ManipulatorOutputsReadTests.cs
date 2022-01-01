using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Tests.Infrastructure.Satel.Mocks;

namespace Sod.Tests.Infrastructure.Satel
{
    public class ManipulatorOutputsReadTests
    {
        // [Theory]
        // [InlineData(
        //     new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D },
        //     CommandStatus.Finished)]
        public async Task GivenBinaryData_WhenFrameIsCorrect_ThenValidResult(byte[] receivedFrame, CommandStatus expectedStatus)
        {
            var segment = new ArraySegment<byte>(receivedFrame);
            var socketSender = new MockSocketSender(frameToSend => Task.FromResult(frameToSend.Length));
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((receivedFrame.Length, segment)));

            var manipulator = new Manipulator(new GenericCommunicationInterface(socketReceiver, socketSender));
            var (status, state) = await manipulator.ReadOutputs();
            status.Should().Be(expectedStatus);
            state[9].Should().BeTrue();
        }
    }
}