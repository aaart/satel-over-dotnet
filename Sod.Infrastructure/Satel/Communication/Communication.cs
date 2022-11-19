using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Socket;

namespace Sod.Infrastructure.Satel.Communication;

public static class Communication
{
    public static async Task<bool> SendAsync(ISocketSender sender, Command cmd, byte[] binaryState, byte[] userCode)
    {
        var isUpdateCommand = (int)cmd >= (int)Command.ArmInMode0 && (int)cmd <= (int)Command.OutputsSwitch;
        if (isUpdateCommand && userCode.Length == 0) throw new InvalidOperationException("No user code was provided!");

        var frame = isUpdateCommand
            ? Frame.Create(cmd, userCode, binaryState)
            : Frame.Create(cmd, binaryState);
        var bytesSentCount = await sender.Send(frame);
        return frame.Length == bytesSentCount;
    }

    public static async Task<(CommandStatus receiveStatus, byte[] data)> ReceiveAsync(ISocketReceiver receiver, Command expectedCommand)
    {
        var (byteCount, segment) = await receiver.ReceiveAsync();
        var data = segment.Array;
        if (data == null || byteCount < 7 || data[0] != 0xFE || data[1] != 0xFE || data[byteCount - 2] != 0xFE || data[byteCount - 1] != 0x0D) return (CommandStatus.InvalidFrame, Array.Empty<byte>());

        var cmd = data[2];
        if (!Enum.IsDefined(typeof(Command), (int)cmd)) return (CommandStatus.NotSupportedCommand, new[] { cmd });

        if ((Command)cmd != expectedCommand) return (CommandStatus.InvalidCommandReceived, new[] { cmd });

        var receivedCrcHigh = data[byteCount - 4];
        var receivedCrcLow = data[byteCount - 3];
        data = segment.Slice(3, byteCount - 7).ToArray();
        var (calculatedCrcHigh, calculatedCrcLow) = Frame.Crc(cmd, data);
        if (receivedCrcHigh != calculatedCrcHigh || receivedCrcLow != calculatedCrcLow) return (CommandStatus.InvalidCrc, new[] { receivedCrcHigh, receivedCrcLow, calculatedCrcHigh, calculatedCrcLow });

        return (CommandStatus.Processed, data);
    }
}