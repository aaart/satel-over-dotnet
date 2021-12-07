using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.Communication
{
    public interface IManipulator
    {
        Task<(CommandStatus status, bool[] outputsState)> ReadOutputs();
        Task<(CommandStatus status, bool[] inputsState)> ReadInputs();
        Task<(CommandStatus status, IntegraResponse response)> SwitchOutputs(bool[] outputs);
        Task<(CommandStatus status, IntegraResponse response)> ArmInMode0(bool[] partitions);
        Task<(CommandStatus status, IntegraResponse response)> DisArm(bool[] partitions);
        Task<(CommandStatus status, bool[] zonesAlarm)> ReadArmedPartitions();
    }
}