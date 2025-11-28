using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Socket;
using static Sod.Infrastructure.Satel.Communication.Communication;

namespace Sod.Infrastructure.Satel.Communication;

public class GenericCommunicationInterface(
    ISocketReceiver socketReceiver,
    ISocketSender socketSender)
    : LoggingCapability
{
    public async Task<(CommandStatus status, TResp)> Execute<TResp>(CommunicationMessage message, CommunicationDefaultResponse<TResp> defaultResponse, Func<byte[], TResp> resultTranslation)
    {
        Logger.LogDebug($"Executing {message.Command.ToString()} command.");
        var sent = await SendAsync(socketSender, message.Command, message.NewState, message.UserCode);
        if (!sent)
        {
            Logger.LogDebug("Command not send. Exiting.");
            return (CommandStatus.NotSent, defaultResponse.Value);
        }

        var (status, data) = await ReceiveAsync(socketReceiver, defaultResponse.ExpectedCommand);
        Logger.LogDebug($"{message.Command.ToString()} executed with {status.ToString()} status.");
        if (status != CommandStatus.Processed)
        {
            Logger.LogWarning($"{message.Command} command execution done with {status} status.");
        }
        return
            status != CommandStatus.Processed
                ? (status, defaultResponse.Value)
                : (CommandStatus.Processed, resultTranslation(data));
    }
}