using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Loop
{
    public interface IStep
    {
        Task ExecuteAsync();
    }
}