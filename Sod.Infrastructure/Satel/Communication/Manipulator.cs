using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Socket;
using static Sod.Infrastructure.Satel.Communication.Communication;

namespace Sod.Infrastructure.Satel.Communication
{
    public class Manipulator : LoggingCapability, IManipulator
    {
        private readonly ISocketSender _socketSender;
        private readonly ISocketReceiver _socketReceiver;
        private readonly byte[] _userCode;

        public Manipulator(ISocketSender socketSender, ISocketReceiver socketReceiver)
            : this(socketSender, socketReceiver, string.Empty)
        {
        }
        
        public Manipulator(ISocketSender socketSender, ISocketReceiver socketReceiver, string userCode)
        {
            _socketSender = socketSender;
            _socketReceiver = socketReceiver;
            _userCode = !string.IsNullOrEmpty(userCode) ? Translation.CreateUserCodeBinaryRepresentation(userCode) : Array.Empty<byte>();
        }
        
        public async Task<(CommandStatus status, bool[] outputsState)> ReadOutputs()
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.OutputsState, Array.Empty<byte>(), Array.Empty<byte>()),
                Array.Empty<bool>(),
                Command.OutputsState,
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, bool[] inputsState)> ReadInputs()
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.ZonesViolation, Array.Empty<byte>(), Array.Empty<byte>()),
                Array.Empty<bool>(),
                Command.ZonesViolation,
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, bool[] zonesAlarm)> ReadZonesAlarm()
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.ZonesAlarm, Array.Empty<byte>(), Array.Empty<byte>()),
                Array.Empty<bool>(),
                Command.ZonesAlarm,
                Translation.ToBooleanArray);
        }
        
        public async Task<(CommandStatus status, bool[] zonesAlarm)> ReadArmedPartitions()
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.ArmedPartitionsReally, Array.Empty<byte>(), Array.Empty<byte>()),
                Array.Empty<bool>(),
                Command.ArmedPartitionsReally,
                Translation.ToBooleanArray);
        }

        public async Task<(CommandStatus status, IntegraResponse response)> SwitchOutputs(bool[] outputs)
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.OutputsSwitch, Translation.ToByteArray(outputs), _userCode),
                IntegraResponse.NotApplicable,
                Command.Result,
                data => (IntegraResponse)data[0]);
        }
        
        public async Task<(CommandStatus status, IntegraResponse response)> ArmInMode0(bool[] partitions)
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.ArmInMode0, Translation.ToByteArray(partitions), _userCode),
                IntegraResponse.NotApplicable,
                Command.Result,
                data => (IntegraResponse)data[0]);
        }
        
        public async Task<(CommandStatus status, IntegraResponse response)> DisArm(bool[] partitions)
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.DisArm, Translation.ToByteArray(partitions), _userCode),
                IntegraResponse.NotApplicable,
                Command.Result,
                data => (IntegraResponse)data[0]);
        }
        
        public async Task<(CommandStatus status, TResp)> GenericResponse<TResp>(Func<Task<bool>> request, TResp defaultResp, Command expectedCommand, Func<byte[], TResp> result)
        {
            var sent = await request();
            if (!sent)
            {
                return (CommandStatus.NotSent, defaultResp);
            }

            var (status, data) = await ReceiveAsync(_socketReceiver, expectedCommand);
            if (status != CommandStatus.Processed)
            {
                return (status, defaultResp);
            }

            return (CommandStatus.Processed, result(data));
        }
    }
}