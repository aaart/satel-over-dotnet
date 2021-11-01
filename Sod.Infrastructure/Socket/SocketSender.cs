using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Socket
{
    public class SocketSender : ISocketSender
    {
        private readonly System.Net.Sockets.Socket _socket;

        public SocketSender(System.Net.Sockets.Socket socket)
        {
            _socket = socket;
        }

        public async Task<int> Send(byte[] data) => await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
    }
}