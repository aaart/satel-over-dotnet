using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Loop
{
    public interface IStepCollectionFactory
    {
        IStepCollection BuildStepCollection();
        Task BuildAndExecuteStepCollection();
    }
}