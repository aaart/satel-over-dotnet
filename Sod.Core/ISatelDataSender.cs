using System.Threading.Tasks;

namespace Sod.Core
{
    public interface ISatelDataSender
    {
        Task<bool> SendAsync(Command cmd, params bool[] state);
    }
}