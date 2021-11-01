using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.Socket
{
    public class SocketReceiver : ISocketReceiver
    {
        private readonly System.Net.Sockets.Socket _socket;

        public SocketReceiver(System.Net.Sockets.Socket socket)
        {
            _socket = socket;
        }

        public async Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync()
        {
            var data = new byte[65]; // 65 = max resp. size - ethm1-plus documentation says 
            var segment = new ArraySegment<byte>(data);
            var byteCount = await _socket.ReceiveAsync(segment, SocketFlags.None);
            return (byteCount, segment);
        }
    }
}