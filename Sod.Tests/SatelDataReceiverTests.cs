using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sod.Core;
using Sod.Tests.Mocks;
using Xunit;

namespace Sod.Tests
{
    public class SatelDataReceiverTests
    {
        [Theory]
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsCorrect_ThenValidResult(byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var satelDataReceiver = new SatelDataReceiver(socketReceiver);
            var (receiveStatus, command, data) = await satelDataReceiver.ReceiveAsync();
            receiveStatus.Should().Be(ReceiveStatus.Success);
            command.Should().Be(Command.OutputsState);
            data.Length.Should().Be(128);
        }
        
        [Theory]
        // data invalid, crc fine (from successful test)
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        // data fine (from successful test), crc invalid
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x2D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsIncorrect_ThenInvalidCrcResult(byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var satelDataReceiver = new SatelDataReceiver(socketReceiver);
            var (receiveStatus, command, data) = await satelDataReceiver.ReceiveAsync();
            receiveStatus.Should().Be(ReceiveStatus.InvalidCrc);
            command.Should().Be(Command.Invalid);
            data.Length.Should().Be(0);
        }
        
        [Theory]
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0A })]
        [InlineData(new byte[] { 0xFE, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFF, 0x0D })]
        [InlineData(new byte[] { 0xFE, 0x0E, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        [InlineData(new byte[] { 0x1E, 0xFE, 0x17, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3D, 0x73, 0xFE, 0x0D })]
        public async void GivenBinaryData_WhenFrameIsIncorrect_ThenInvalidFrameResult1(byte[] frame)
        {
            var segment = new ArraySegment<byte>(frame);
            var socketReceiver = new MockSocketReceiver(() => Task.FromResult((frame.Length, segment)));
            var satelDataReceiver = new SatelDataReceiver(socketReceiver);
            var (receiveStatus, command, data) = await satelDataReceiver.ReceiveAsync();
            receiveStatus.Should().Be(ReceiveStatus.InvalidFrame);
            command.Should().Be(Command.Invalid);
            data.Length.Should().Be(0);
        }
    }
}