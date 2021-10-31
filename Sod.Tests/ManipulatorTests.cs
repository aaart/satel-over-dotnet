using System;
using System.Threading.Tasks;
using Sod.Core;
using Sod.Tests.Mocks;
using Xunit;

namespace Sod.Tests
{
    public class ManipulatorTests
    {
        [Theory]
        [InlineData(
            new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D },
            CommandStatus.SuccessfulRead,
            Command.OutputsState)]
        public async Task GivenBinaryData_WhenFrameIsCorrect_ThenValidResult(byte[] receivedFrame, CommandStatus expectedStatus, Command expectedCommand)
        {
            var segment = new ArraySegment<byte>(receivedFrame);
            var sockerSender = new MockSocketSender(frameToSend => Task.FromResult(frameToSend.Length));
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((receivedFrame.Length, segment)));

            var manipulator = new Manipulator(sockerSender, socketReceiver, Array.Empty<byte>());
            var (status, state) = await manipulator.ReadOutputsState();
            
        }
    }
}