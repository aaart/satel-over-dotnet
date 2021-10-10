using System;
using FluentAssertions;
using Sod.Core;
using Xunit;

namespace Sod.Tests
{
    public class FrameCalculatorTests
    {
        [Theory]
        [InlineData(ReadCommand.ArmedPartitionsSuppressed, new byte[] { 0xFE, 0xFE, 0x09, 0xD7, 0xEB, 0xFE, 0x0D })]
        public void ReadFrames(ReadCommand cmd, byte[] expectedFrame)
        {
            var frame = FrameCalculator.CreateReadFrame(cmd);
            frame.Should().BeEquivalentTo(expectedFrame);
        }
    }
}