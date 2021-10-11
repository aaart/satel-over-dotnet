using System.Threading.Tasks;

namespace Sod.Core
{
    public interface ISatelConnection
    {
        Task SendAsync(Command cmd, params bool[] state);
        Task<(Command, bool[])> ReceiveAsync();
    }
}