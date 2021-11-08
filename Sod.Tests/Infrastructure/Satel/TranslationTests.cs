using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sod.Infrastructure.Satel;
using Xunit;

namespace Sod.Tests.Infrastructure.Satel
{
    public class TranslationTests
    {
        [Theory]
        [MemberData(nameof(CreateFromLogicToBinaryTestData))]
        public void GivenLogicState_WhenSpecificBinaryStateIsExpected_ThenSuccess(bool[] logicState, byte[] expectedBinaryState)
        {
            var calculatedBitState = Translation.ToByteArray(logicState);
            calculatedBitState.Should().BeEquivalentTo(expectedBinaryState);
        }

        [Theory]
        [MemberData(nameof(CreateFromBinaryToLogicTestData))]
        public void GivenBinaryState_WhenSpecificLogicStateIsExpected_ThenSuccess(byte[] binaryState, bool[] expectedLogicState)
        {
            var calculatedLogicState = Translation.ToBooleanArray(binaryState);
            calculatedLogicState.Should().BeEquivalentTo(expectedLogicState);
        }

        [Theory]
        [InlineData("1234", new byte[] { 0x12, 0x34, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
        public void GivenUserCode_WhenItsCorrect_ThenSucess(string userCode, byte[] expectedBinaryCode)
        {
            Translation.CreateUserCodeBinaryRepresentation(userCode).Should().BeEquivalentTo(expectedBinaryCode);
        }

        public static IEnumerable<object[]> CreateFromLogicToBinaryTestData()
        {
            // all states enabled
            yield return new object[]
            {
                Enumerable.Repeat(true, Translation.SupportedLogicStateArrayLength1).ToArray(),
                Enumerable.Repeat((byte)255, Translation.SupportedBinaryStateArrayLength1).ToArray()
            };

            // all states disabled
            yield return new object[]
            {
                Enumerable.Repeat(false, Translation.SupportedLogicStateArrayLength1).ToArray(),
                Enumerable.Repeat((byte)0, Translation.SupportedBinaryStateArrayLength1).ToArray()
            };

            // only some states enabled, no. 1          
            yield return new object[]
            {
                Enumerable.Repeat(new[] { true, false, false, false, false, false, false, false }, Translation.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray(),
                Enumerable.Repeat((byte)1, Translation.SupportedBinaryStateArrayLength1).ToArray()
            };

            // only some states enabled, no. 2          
            yield return new object[]
            {
                Enumerable.Repeat(new[] { false, true, true, false, false, false, false, true }, Translation.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray(),
                Enumerable.Repeat((byte)134, Translation.SupportedBinaryStateArrayLength1).ToArray()
            };

            // only some states enabled, no. 3          
            yield return new object[]
            {
                Enumerable.Repeat(new[] { false, true, true, false, false, false, false, true }, Translation.SupportedBinaryStateArrayLength1 - 1).SelectMany(x => x)
                    .Concat(new[] { true, false, false, true, true, true, true, false }).ToArray(),
                Enumerable.Repeat((byte)134, Translation.SupportedBinaryStateArrayLength1 - 1)
                    .Concat(new[] { (byte)121 }).ToArray()
            };
        }

        public static IEnumerable<object[]> CreateFromBinaryToLogicTestData()
        {
            // all states enabled
            yield return new object[]
            {
                Enumerable.Repeat((byte)255, Translation.SupportedBinaryStateArrayLength1).ToArray(),
                Enumerable.Repeat(true, Translation.SupportedLogicStateArrayLength1).ToArray()
            };

            // all states disabled
            yield return new object[]
            {
                Enumerable.Repeat((byte)0, Translation.SupportedBinaryStateArrayLength1).ToArray(),
                Enumerable.Repeat(false, Translation.SupportedLogicStateArrayLength1).ToArray()
            };

            // only some states enabled, no. 1          
            yield return new object[]
            {
                Enumerable.Repeat((byte)1, Translation.SupportedBinaryStateArrayLength1).ToArray(),
                Enumerable.Repeat(new[] { true, false, false, false, false, false, false, false }, Translation.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray()
            };

            // only some states enabled, no. 2          
            yield return new object[]
            {
                Enumerable.Repeat((byte)134, Translation.SupportedBinaryStateArrayLength1).ToArray(),
                Enumerable.Repeat(new[] { false, true, true, false, false, false, false, true }, Translation.SupportedBinaryStateArrayLength1).SelectMany(x => x).ToArray()
            };

            // only some states enabled, no. 3          
            yield return new object[]
            {
                Enumerable.Repeat((byte)134, Translation.SupportedBinaryStateArrayLength1 - 1)
                    .Concat(new[] { (byte)121 }).ToArray(),
                Enumerable.Repeat(new[] { false, true, true, false, false, false, false, true }, Translation.SupportedBinaryStateArrayLength1 - 1).SelectMany(x => x)
                    .Concat(new[] { true, false, false, true, true, true, true, false }).ToArray()
            };

            yield return new object[]
            {
                Enumerable.Repeat((byte)134, Translation.SupportedBinaryStateArrayLength1 - 1)
                    .Concat(new[] { (byte)65 }).ToArray(),
                Enumerable.Repeat(new[] { false, true, true, false, false, false, false, true }, Translation.SupportedBinaryStateArrayLength1 - 1).SelectMany(x => x)
                    .Concat(new[] { true, false, false, false, false, false, true, false }).ToArray()
            };
            
            yield return new object[]
            {
                new[] { (byte)5 },
                new[] { true, false, true, false, false, false, false, false }
            };
        }
    }
}