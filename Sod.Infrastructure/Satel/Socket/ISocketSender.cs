using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.Socket;

public interface ISocketSender
{
    Task<int> Send(byte[] data);
}