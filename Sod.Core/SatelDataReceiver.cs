using System;
using System.Threading.Tasks;
using Sod.Core.Communication;

namespace Sod.Core
{
    public class SatelDataReceiver : ISatelDataReceiver
    {
        private readonly ISocketReceiver _socketReceiver;

        public SatelDataReceiver(ISocketReceiver socketReceiver)
        {
            _socketReceiver = socketReceiver;
        }
        
        public async Task<(ReceiveStatus receiveStatus, Command command, bool[] data)> ReceiveAsync()
        {
            var (byteCount, segment) = await _socketReceiver.ReceiveAsync();
            var data = segment.ToArray();
            if (byteCount < 5 || data[0] != 0xFE || data[1] != 0xFE || data[byteCount - 2] != 0xFE || data[byteCount - 1] != 0x0D)
            {
                return (ReceiveStatus.InvalidFrame, Command.Invalid, Array.Empty<bool>());
            }
            if (!Enum.IsDefined(typeof(Command), (int)data[2]))
            {
                return (ReceiveStatus.NotSupportedCommand, Command.Invalid, Array.Empty<bool>());
            }
            
            var cmd = data[2];
            var receivedCrcHigh = data[byteCount - 4];
            var receivedCrcLow = data[byteCount - 3];
            var binaryState = segment.Slice(3, byteCount - 7).ToArray();
            var (calculatedCrcHigh, calculatedCrcLow) = Frame.Crc(cmd, binaryState);
            if (receivedCrcHigh != calculatedCrcHigh || receivedCrcLow != calculatedCrcLow)
            {
                return (ReceiveStatus.InvalidCrc, Command.Invalid, Array.Empty<bool>());
            }
            
            return (ReceiveStatus.Success, (Command)cmd, StateMapping.ToBooleanArray(binaryState));
        }
    }
}