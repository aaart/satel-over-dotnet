using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel.Socket;

public class SocketSender(ISocketConnection connection) : ISocketSender
{
    public async Task<int> Send(byte[] data)
    {
        return await connection.Instance.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
    }
}