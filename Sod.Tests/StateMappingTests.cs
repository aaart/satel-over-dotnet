using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sod.Core;
using Xunit;

namespace Sod.Tests
{
    public class StateMappingTests
    {
        [Theory]
        [MemberData(nameof(FromLogicToBinaryTestData))]
        public void VerifyCalculatedBinaryState(bool[] logicState, byte[] expectedBitState)
        {
            var calculatedBitState = StateMapping.ToByteArray(logicState);
            calculatedBitState.Should().BeEquivalentTo(expectedBitState);
        }

        [Theory]
        [MemberData(nameof(FromBinaryToLogicTestData))]
        public void VerifyCalculatedLogicState(byte[] binaryState, bool[] expectedLogicState)
        {
            var calculatedLogicState = StateMapping.ToBooleanArray(binaryState);
            calculatedLogicState.Should().BeEquivalentTo(expectedLogicState);
        }

        public static IEnumerable<object[]> FromLogicToBinaryTestData
        {
            get
            {
                // all states enabled
                yield return new object[]
                {
                    Enumerable.Repeat(true, StateMapping.SupportedLogicStateArrayLength1).ToArray(),
                    Enumerable.Repeat((byte) 255, StateMapping.SupportedBinaryStateArrayLength1).ToArray()
                };

                // all states disabled
                yield return new object[]
                {
                    Enumerable.Repeat(false, StateMapping.SupportedLogicStateArrayLength1).ToArray(),
                    Enumerable.Repeat((byte) 0, StateMapping.SupportedBinaryStateArrayLength1).ToArray()
                };

                // only some states enabled, no. 1          
                yield return new object[]
                {
                    Enumerable.Repeat(new[] {true, false, false, false, false, false, false, false}, StateMapping.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray(),
                    Enumerable.Repeat((byte) 128, StateMapping.SupportedBinaryStateArrayLength1).ToArray()
                };

                // only some states enabled, no. 2          
                yield return new object[]
                {
                    Enumerable.Repeat(new[] {false, true, true, false, false, false, false, true}, StateMapping.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray(),
                    Enumerable.Repeat((byte) 97, StateMapping.SupportedBinaryStateArrayLength1).ToArray()
                };

                // only some states enabled, no. 3          
                yield return new object[]
                {
                    Enumerable.Repeat(new[] {false, true, true, false, false, false, false, true}, StateMapping.SupportedBinaryStateArrayLength1 - 1).SelectMany(x => x)
                        .Concat(new[] {true, false, false, true, true, true, true, false}).ToArray(),
                    Enumerable.Repeat((byte) 97, StateMapping.SupportedBinaryStateArrayLength1 - 1)
                        .Concat(new[] {(byte) 158}).ToArray()
                };
            }
        }
        
        public static IEnumerable<object[]> FromBinaryToLogicTestData
        {
            get
            {
                // all states enabled
                yield return new object[]
                {
                    Enumerable.Repeat((byte) 255, StateMapping.SupportedBinaryStateArrayLength1).ToArray(),
                    Enumerable.Repeat(true, StateMapping.SupportedLogicStateArrayLength1).ToArray()
                };

                // all states disabled
                yield return new object[]
                {
                    Enumerable.Repeat((byte) 0, StateMapping.SupportedBinaryStateArrayLength1).ToArray(),
                    Enumerable.Repeat(false, StateMapping.SupportedLogicStateArrayLength1).ToArray()
                };

                // only some states enabled, no. 1          
                yield return new object[]
                {
                    Enumerable.Repeat((byte) 128, StateMapping.SupportedBinaryStateArrayLength1).ToArray(),
                    Enumerable.Repeat(new[] {true, false, false, false, false, false, false, false}, StateMapping.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray()
                };

                // only some states enabled, no. 2          
                yield return new object[]
                {
                    Enumerable.Repeat((byte) 97, StateMapping.SupportedBinaryStateArrayLength1).ToArray(),
                    Enumerable.Repeat(new[] {false, true, true, false, false, false, false, true}, StateMapping.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray()
                };

                // only some states enabled, no. 3          
                yield return new object[]
                {
                    Enumerable.Repeat((byte) 97, StateMapping.SupportedBinaryStateArrayLength1 - 1)
                        .Concat(new[] {(byte) 158}).ToArray(),
                    Enumerable.Repeat(new[] {false, true, true, false, false, false, false, true}, StateMapping.SupportedBinaryStateArrayLength1 - 1).SelectMany(x => x)
                        .Concat(new[] {true, false, false, true, true, true, true, false}).ToArray()
                };
            }
        }
    }
}