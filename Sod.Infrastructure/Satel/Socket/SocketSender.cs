using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel.Socket;

public class SocketSender : ISocketSender
{
    private readonly ISocketConnection _connection;

    public SocketSender(ISocketConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> Send(byte[] data)
    {
        return await _connection.Instance.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
    }
}