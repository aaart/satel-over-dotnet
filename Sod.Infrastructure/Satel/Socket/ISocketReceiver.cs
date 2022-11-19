using System;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.Socket;

public interface ISocketReceiver
{
    Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync();
}