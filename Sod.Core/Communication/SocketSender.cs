using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sod.Core.Communication
{
    public class SocketSender : ISocketSender
    {
        private readonly Socket _socket;

        public SocketSender(Socket socket)
        {
            _socket = socket;
        }

        public async Task<int> Send(byte[] data) => await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
    }
}