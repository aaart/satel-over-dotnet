using System.Threading.Tasks;

namespace Sod.Infrastructure.Socket
{
    public interface ISocketSender
    {
        Task<int> Send(byte[] data);
    }
}