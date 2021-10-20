using System;
using System.Threading.Tasks;
using Sod.Core.Communication;

namespace Sod.Core
{
    public class SatelDataSender : ISatelDataSender
    {
        private readonly ISocketSender _socketSender;
        private readonly byte[] _userCode;

        public SatelDataSender(ISocketSender socketSender)
            : this(socketSender, Array.Empty<byte>())
        {
        }
        
        public SatelDataSender(ISocketSender socketSender, byte[] userCode)
        {
            _socketSender = socketSender;
            _userCode = userCode;
        }

        public async Task<bool> SendAsync(Command cmd) => await SendAsyncImpl(cmd, Array.Empty<byte>());

        public Task<bool> SendAsync(Command cmd, bool[] state) => SendAsyncImpl(cmd, Translation.ToByteArray(state));

        private async Task<bool> SendAsyncImpl(Command cmd, byte[] binaryState)
        {
            var isUpdateCommand = IsUpdateCommand(cmd);
            if (isUpdateCommand && _userCode.Length == 0)
            {
                throw new InvalidOperationException("No user code was provided!");
            }
            
            byte[] frame = isUpdateCommand
                ? Frame.Create(cmd, _userCode, binaryState) 
                : Frame.Create(cmd, binaryState);
            var bytesSentCount = await _socketSender.Send(frame);
            return frame.Length == bytesSentCount;
        }

        private static bool IsUpdateCommand(Command cmd) => (int)cmd >= (int)Command.ArmInMode0 && (int)cmd <= (int)Command.OutputsSwitch;
    }
}