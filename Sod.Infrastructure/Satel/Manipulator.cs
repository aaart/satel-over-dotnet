using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Socket;
using static Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel
{
    public class Manipulator : IManipulator
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

        public async Task<(CommandStatus status, IntegraResponse response)> SwitchOutputs(bool[] outputs)
        {
            return await GenericResponse(
                async () => await SendAsync(_socketSender, Command.OutputsSwitch, Translation.ToByteArray(outputs), _userCode),
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
            if (status != CommandStatus.Finished)
            {
                return (status, defaultResp);
            }

            return (CommandStatus.Finished, result(data));
        }
    }
}