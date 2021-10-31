using System.Threading.Tasks;

namespace Sod.Core
{
    public interface IManipulator
    {
        Task<(CommandStatus status, bool[] outputsState)> ReadOutputsState();
        Task<(CommandStatus status, bool[] inputsState)> ReadInputsState();
    }
}