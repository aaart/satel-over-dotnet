using System;
using System.Linq;

namespace Sod.Core
{
    public static class FrameCalculator
    {
        public static byte[] CreateFrame(Command cmd, params byte[] data) => CreateFrame((byte)cmd, data);
        
        private static byte[] CreateFrame(byte cmd, params byte[] data)
        {
            ushort crc = 0x147A;
            foreach (byte element in new[] {cmd}.Concat(data))
            {
                crc = (ushort) ((crc << 1) | (crc >> 15));
                crc ^= 0xFFFF;
                crc = (ushort) (crc + CrcHigh(crc) + element);
            }

            return new byte[] {0xFE, 0xFE, cmd, CrcHigh(crc), CrcLow(crc), 0xFE, 0x0D};
        }

        private static byte CrcHigh(ushort crc) => (byte) (crc >> 8);

        private static byte CrcLow(ushort crc) => (byte) crc;
    }
}