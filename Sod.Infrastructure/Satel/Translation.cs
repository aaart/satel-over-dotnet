using System;
using System.Linq;

namespace Sod.Infrastructure.Satel
{
    public static class Translation
    {
        private const int ByteSize = 8;
        public const int SupportedLogicStateArrayLength1 = 128;
        public const int SupportedLogicStateArrayLength2 = 256;
        public const int SupportedBinaryStateArrayLength1 = SupportedLogicStateArrayLength1 / ByteSize;
        public const int SupportedBinaryStateArrayLength2 = SupportedLogicStateArrayLength2 / ByteSize;

        public static byte[] CreateUserCodeBinaryRepresentation(string userCode)
        {
            byte GetBinaryCode(char c) => Convert.ToByte(Convert.ToByte(c) - 48); // why 48? check ASCII table

            var bytes = Enumerable.Repeat((byte)0xFF, 8).ToArray();
            userCode = userCode.Length % 2 == 0 ? userCode : "0" + userCode;
            for (int i = 0; i < userCode.Length; i += 2)
            {
                bytes[i / 2] = (byte)((GetBinaryCode(userCode[i]) << 4) | GetBinaryCode(userCode[i + 1]));
            }
            
            return bytes;
        }
        
        public static byte[] ToByteArray(bool[] logicState)
        {
            if (logicState.Length != 0 && logicState.Length != SupportedLogicStateArrayLength1 && logicState.Length != SupportedLogicStateArrayLength2)
            {
                throw new ArgumentException($"{logicState.Length} is not supported length", nameof(logicState));
            }

            var binaryStateLength = logicState.Length / ByteSize;
            var binaryState = new byte[binaryStateLength];
            for (int i = 0; i < binaryStateLength; i++)
            {
                for (int j = 0; j < ByteSize; j++)
                {
                    if (logicState[i * ByteSize + j])
                    {
                        binaryState[i] = (byte)(binaryState[i] | (0b0000_0001 << j));
                    }
                }
            }

            return binaryState;
        }

        public static bool[] ToBooleanArray(byte[] binaryState)
        {
            var logicStateLength = binaryState.Length * ByteSize;
            var logicState = new bool[logicStateLength];
            for (int i = 0; i < binaryState.Length; i++)
            {
                for (int j = 0; j < ByteSize; j++)
                {
                    var decimalValue = (byte)(0b0000_0001 << j);
                    if ((binaryState[i] & decimalValue) == decimalValue)
                    {
                        logicState[i * ByteSize + j] = true;
                    }
                }
            }

            return logicState;
        }
    }
}