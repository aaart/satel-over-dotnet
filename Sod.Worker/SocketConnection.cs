using System;
using System.Net.Sockets;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Worker;

public class SocketConnection : ISocketConnection, IDisposable
{
    private readonly SatelConnectionOptions _options;
    private Socket _socket;
    private object _lock = new();

    private Func<SocketConnection, Socket> _onSocketPropertyCalled = facade =>
    {
        lock (facade._lock)
        {
            facade.Connect();
            facade._onSocketPropertyCalled = GetSocket;
            return facade._socket;
        }
    };

    private static Socket GetSocket(SocketConnection connection)
    {
        return connection._socket;
    }

    public SocketConnection(SatelConnectionOptions options)
    {
        _options = options;
    }

    public Socket Instance => _onSocketPropertyCalled(this);

    public void Connect()
    {
        _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(_options.Address, _options.Port);
    }

    public void Reconnect()
    {
        // if (_socket.is)
        // {
        //     
        // }
        _socket.Dispose();
        Connect();
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}