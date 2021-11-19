using System.Threading.Tasks;

namespace Sod.Infrastructure.State.Loop
{
    public interface IStep
    {
        Task ExecuteAsync();
    }
}