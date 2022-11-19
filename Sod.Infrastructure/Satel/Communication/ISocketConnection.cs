namespace Sod.Infrastructure.Satel.Communication;

public interface ISocketConnection
{
    System.Net.Sockets.Socket Instance { get; }
    void Reconnect();
}