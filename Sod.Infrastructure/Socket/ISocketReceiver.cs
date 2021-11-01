using System;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Socket
{
    public interface ISocketReceiver
    {
        Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync();
    }
}