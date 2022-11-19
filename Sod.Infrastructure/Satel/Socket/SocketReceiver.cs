using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel.Socket;

public class SocketReceiver : ISocketReceiver
{
    private readonly ISocketConnection _connection;

    public SocketReceiver(ISocketConnection connection)
    {
        _connection = connection;
    }

    public async Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync()
    {
        var data = new byte[65]; // 65 = max resp. size - ethm1-plus documentation says 
        var segment = new ArraySegment<byte>(data);
        var byteCount = await _connection.Instance.ReceiveAsync(segment, SocketFlags.None);
        return (byteCount, segment);
    }
}