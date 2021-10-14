using System;
using System.Threading.Tasks;

namespace Sod.Core.Communication
{
    public interface ISocketReceiver
    {
        Task<(int byteCount, ArraySegment<byte> receivedBinaryData)> ReceiveAsync();
    }
}