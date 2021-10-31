using System.Threading.Tasks;

namespace Sod.Core.Socket
{
    public interface ISocketSender
    {
        Task<int> Send(byte[] data);
    }
}