using System;
using System.Linq;

namespace Sod.Infrastructure.Satel
{
    public static class Frame
    {
        public static byte[] Create(Command cmd, byte[] userCode, byte[] data) => Create((byte)cmd, userCode, data);
        public static byte[] Create(Command cmd, params byte[] data) => Create((byte)cmd, Array.Empty<byte>(), data);

        public static (byte crcHigh, byte crcLow) Crc(byte cmd, byte[] data)
        {
            byte CrcHigh(ushort crc) => (byte)(crc >> 8);
            byte CrcLow(ushort crc) => (byte)crc;

            ushort crc = 0x147A;
            foreach (byte element in new []{ cmd }.Concat(data))
            {
                crc = (ushort)((crc << 1) | (crc >> 15));
                crc ^= 0xFFFF;
                crc = (ushort)(crc + CrcHigh(crc) + element);
            }

            return (CrcHigh(crc), CrcLow(crc));
        }

        private static byte[] Create(byte cmd, byte[] userCode, byte[] data)
        {
            var userCodeAndData = userCode.Concat(data).ToArray();
            var (crcHigh, crcLow) = Crc(cmd, userCodeAndData);
            return new byte[] { 0xFE, 0xFE, cmd }
                .Concat(userCodeAndData)
                .Concat(new byte[] { crcHigh, crcLow, 0xFE, 0x0D }).ToArray();
        }
    }
}