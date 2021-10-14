using System;
using FluentAssertions;
using Sod.Core;
using Xunit;

namespace Sod.Tests
{
    public class FrameTests
    {
        [Theory]
        [InlineData(Command.ArmedPartitionsSuppressed, new byte[] { 0xFE, 0xFE, 0x09, 0xD7, 0xEB, 0xFE, 0x0D })]
        public void ReadFrames(Command cmd, byte[] expectedFrame)
        {
            var frame = Frame.Create(cmd);
            frame.Should().BeEquivalentTo(expectedFrame);
        }
    }
}