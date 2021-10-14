using System.Threading.Tasks;

namespace Sod.Core
{
    public interface ISatelDataReceiver
    {
        Task<(ReceiveStatus receiveStatus, Command command, bool[] data)> ReceiveAsync();
    }
}