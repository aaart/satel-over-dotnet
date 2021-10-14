using System;
using System.Threading.Tasks;
using Sod.Core.Communication;

namespace Sod.Core
{
    public class SatelDataSender : ISatelDataSender
    {
        private readonly ISocketSender _socketSender;

        public SatelDataSender(ISocketSender socketSender)
        {
            _socketSender = socketSender;
        }

        public async Task<bool> SendAsync(Command cmd, params bool[] state)
        {
            var binaryData = StateMapping.ToByteArray(state);
            var frame = Frame.Create(cmd, binaryData);
            var bytesSentCount = await _socketSender.Send(frame);
            if (frame.Length != bytesSentCount)
            {
                return false;
            }
            return true;
        }
    }
}