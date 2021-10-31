using System;
using System.Linq;
using System.Threading.Tasks;
using Sod.Core.Socket;
using static Sod.Core.Communication;

namespace Sod.Core
{
    public class Manipulator : IManipulator
    {
        private readonly ISocketSender _socketSender;
        private readonly ISocketReceiver _socketReceiver;
        private readonly byte[] _userCode;
        
        public Manipulator(ISocketSender socketSender, ISocketReceiver socketReceiver, byte[] userCode)
        {
            _socketSender = socketSender;
            _socketReceiver = socketReceiver;
            _userCode = userCode;
        }
        
        public async Task<(CommandStatus status, bool[] outputsState)> ReadOutputsState()
        {
            var sent = await SendAsync(_socketSender, Command.OutputsState, Array.Empty<byte>(), Array.Empty<byte>());
            if (!sent)
            {
                return (CommandStatus.NotSent, Array.Empty<bool>());
            }

            var (status, data) = await ReceiveAsync(_socketReceiver, Command.OutputsState);
            if (status != CommandStatus.SuccessfulRead)
            {
                return (status, Array.Empty<bool>());
            }

            var state = Translation.ToBooleanArray(data);
            return (CommandStatus.SuccessfulRead, state);
        }

        public async Task<(CommandStatus status, bool[] inputsState)> ReadInputsState()
        {
            var sent = await SendAsync(_socketSender, Command.ZonesViolation, Array.Empty<byte>(), Array.Empty<byte>());
            if (!sent)
            {
                return (CommandStatus.NotSent, Array.Empty<bool>());
            }

            var (status, data) = await ReceiveAsync(_socketReceiver, Command.ZonesViolation);
            if (status != CommandStatus.SuccessfulRead)
            {
                return (status, Array.Empty<bool>());
            }

            var state = Translation.ToBooleanArray(data);
            return (CommandStatus.SuccessfulRead, state);
        }
    }
}