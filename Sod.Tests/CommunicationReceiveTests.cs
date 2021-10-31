using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sod.Core;
using Sod.Tests.Mocks;
using Xunit;
using static Sod.Core.Communication;

namespace Sod.Tests
{
    public class CommunicationReceiveTests
    {
        [Theory]
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsCorrect_ThenValidResult(byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var (receiveStatus, data) = await ReceiveAsync(socketReceiver, Command.OutputsState);
            receiveStatus.Should().Be(CommandStatus.SuccessfulRead);
            data.Length.Should().Be(16);
        }
        
        [Theory]
        // data invalid, crc fine (from successful test)
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        // data fine (from successful test), crc invalid
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x2D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsIncorrect_ThenInvalidCrcResult(Command expectedCommand, byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var (receiveStatus, data) = await ReceiveAsync(socketReceiver, expectedCommand);
            receiveStatus.Should().Be(CommandStatus.InvalidCrc);
            data.Length.Should().Be(0);
        }
        
        [Theory]
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C })]
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0A })]
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFF, 0x0D })]
        [InlineData(Command.OutputsState, new byte[] { 0xFE, 0x0E, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        [InlineData(Command.OutputsState, new byte[] { 0x1E, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsIncorrect_ThenInvalidFrameResult1(Command expectedCommand, byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var (receiveStatus, data) = await ReceiveAsync(socketReceiver, expectedCommand);
            receiveStatus.Should().Be(CommandStatus.InvalidFrame);
            data.Length.Should().Be(0);
        }
    }
}