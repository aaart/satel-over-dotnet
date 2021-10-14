using System.Threading.Tasks;

namespace Sod.Core.Communication
{
    public interface ISocketSender
    {
        Task<int> Send(byte[] data);
    }
}