using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Socket;

namespace Sod.Infrastructure.Satel.Communication
{
    public class Manipulator : LoggingCapability, IManipulator
    {
        private readonly byte[] _userCode;
        private readonly GenericCommunicationInterface _genericCommunicationInterface;

        public Manipulator(GenericCommunicationInterface genericCommunicationInterface)
            : this(genericCommunicationInterface, string.Empty)
        {
            _genericCommunicationInterface = genericCommunicationInterface;
        }
        
        public Manipulator(GenericCommunicationInterface genericCommunicationInterface, string userCode)
        {
            _genericCommunicationInterface = genericCommunicationInterface;
            _userCode = !string.IsNullOrEmpty(userCode) ? Translation.CreateUserCodeBinaryRepresentation(userCode) : Array.Empty<byte>();
        }
        
        public async Task<(CommandStatus status, bool[] outputsState)> ReadOutputs()
        {
            Logger.LogDebug("reading outputs");
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.OutputsState, Array.Empty<byte>(), Array.Empty<byte>()),
                new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.OutputsState),
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, bool[] inputsState)> ReadInputs()
        {
            Logger.LogDebug("reading inputs");
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.ZonesViolation, Array.Empty<byte>(), Array.Empty<byte>()),
                new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.ZonesViolation),
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, bool[] zonesAlarm)> ReadZonesAlarm()
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.ZonesAlarm, Array.Empty<byte>(), Array.Empty<byte>()),
                new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.ZonesAlarm),
                Translation.ToBooleanArray);
        }
        
        public async Task<(CommandStatus status, bool[] zonesAlarm)> ReadArmedPartitions()
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.ArmedPartitionsReally, Array.Empty<byte>(), Array.Empty<byte>()),
                new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.ArmedPartitionsReally),
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, IntegraResponse response)> SwitchOutputs(bool[] outputs)
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.OutputsSwitch, Translation.ToByteArray(outputs), _userCode),
                new CommunicationDefaultResponse<IntegraResponse>(IntegraResponse.NotApplicable, Command.Result),
                data => (IntegraResponse)data[0]);
        }

        public async Task<(CommandStatus status, IntegraResponse response)> DisableOutputs(bool[] outputs)
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.OutputsOff, Translation.ToByteArray(outputs), _userCode),
                new CommunicationDefaultResponse<IntegraResponse>(IntegraResponse.NotApplicable, Command.Result),
                data => (IntegraResponse)data[0]);
        }
        public async Task<(CommandStatus status, IntegraResponse response)> EnableOutputs(bool[] outputs)
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.OutputsOn, Translation.ToByteArray(outputs), _userCode),
                new CommunicationDefaultResponse<IntegraResponse>(IntegraResponse.NotApplicable, Command.Result),
                data => (IntegraResponse)data[0]);
        }

        public async Task<(CommandStatus status, IntegraResponse response)> ArmInMode0(bool[] partitions)
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.ArmInMode0, Translation.ToByteArray(partitions), _userCode),
                new CommunicationDefaultResponse<IntegraResponse>(IntegraResponse.NotApplicable, Command.Result),
                data => (IntegraResponse)data[0]);
        }
        
        public async Task<(CommandStatus status, IntegraResponse response)> DisArm(bool[] partitions)
        {
            return await _genericCommunicationInterface.Execute(
                new CommunicationMessage(Command.DisArm, Translation.ToByteArray(partitions), _userCode),
                new CommunicationDefaultResponse<IntegraResponse>(IntegraResponse.NotApplicable, Command.Result),
                data => (IntegraResponse)data[0]);
        }
    }
}