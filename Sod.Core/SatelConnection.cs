using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sod.Core
{
    public class SatelConnection : ISatelConnection
    {
        private readonly Socket _socket;

        public SatelConnection(Socket socket)
        {
            _socket = socket;
        }

        public async Task SendAsync(Command cmd, params bool[] state)
        {
            var binaryData = StateMapping.ToByteArray(state);
            var frame = FrameCalculator.CreateFrame(cmd, binaryData);
            var bytesSentCount = await _socket.SendAsync(new ArraySegment<byte>(frame), SocketFlags.None);
            if (frame.Length != bytesSentCount)
            {
                throw new InvalidOperationException("Incorrect frame has been sent.");
            }
        }

        public async Task<(Command, bool[])> ReceiveAsync()
        {
            var data = new byte[65]; // 65 = max resp. size
            var segment = new ArraySegment<byte>(data);
            var byteCount = await _socket.ReceiveAsync(segment, SocketFlags.None);
            if (byteCount < 5)
            {
                throw new InvalidDataException("The response is to short");
            }

            if (data[0] != 0xFE || data[1] != 0xFE)
            {
                throw new InvalidDataException("Invalid response.");
            }
            if (!Enum.IsDefined(typeof(Command), (int)data[2]))
            {
                throw new InvalidCastException($"{data[2]} is not supported command!");
            }
            var cmd = (Command) data[2];

            return (cmd, StateMapping.ToBooleanArray(segment.Slice(3, byteCount - 7).ToArray()));
        }
    }
}