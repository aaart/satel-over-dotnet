using System.Threading.Tasks;

namespace Sod.Infrastructure
{
    public interface IManipulator
    {
        Task<(CommandStatus status, bool[] outputsState)> ReadOutputs();
        Task<(CommandStatus status, bool[] inputsState)> ReadInputs();
        Task<(CommandStatus status, IntegraResponse response)> SwitchOutputs(bool[] outputs);
    }
}