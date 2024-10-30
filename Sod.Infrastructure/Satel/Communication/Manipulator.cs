using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;

namespace Sod.Infrastructure.Satel.Communication;

public class Manipulator : LoggingCapability, IManipulator
{
    private readonly byte[] _userCode;
    private readonly GenericCommunicationInterface _genericCommunicationInterface;


    public Manipulator(GenericCommunicationInterface genericCommunicationInterface, SatelUserCodeOptions options)
    {
        _genericCommunicationInterface = genericCommunicationInterface;
        _userCode = (!string.IsNullOrEmpty(options.UserCode) ? Translation.CreateUserCodeBinaryRepresentation(options.UserCode) : null) ?? throw new ArgumentException("Provided user code is empty.");
    }

    public async Task<(CommandStatus status, bool[] outputsState)> ReadOutputs()
    {
        return await _genericCommunicationInterface.Execute(
            new CommunicationMessage(Command.OutputsState, Array.Empty<byte>(), Array.Empty<byte>()),
            new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.OutputsState),
            Translation.ToBooleanArray);
    }

    public async Task<(CommandStatus status, bool[] inputsState)> ReadInputs()
    {
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

    public async Task<(CommandStatus status, bool[] triggeredZones)> ReadAlarmTriggered()
    {
        return await _genericCommunicationInterface.Execute(
            new CommunicationMessage(Command.PartitionsAlarm, Array.Empty<byte>(), Array.Empty<byte>()),
            new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.PartitionsAlarm),
            Translation.ToBooleanArray);
    }

    public async Task<(CommandStatus, bool[])> ReadSuppressedPartitions()
    {
        return await _genericCommunicationInterface.Execute(
            new CommunicationMessage(Command.ArmedPartitionsSuppressed, Array.Empty<byte>(), Array.Empty<byte>()),
            new CommunicationDefaultResponse<bool[]>(Array.Empty<bool>(), Command.ArmedPartitionsSuppressed),
            Translation.ToBooleanArray);
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