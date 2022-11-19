using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Socket;
using static Sod.Infrastructure.Satel.Communication.Communication;

namespace Sod.Infrastructure.Satel.Communication;

public class GenericCommunicationInterface : LoggingCapability
{
    private readonly ISocketReceiver _socketReceiver;
    private readonly ISocketSender _socketSender;

    public GenericCommunicationInterface(
        ISocketReceiver socketReceiver,
        ISocketSender socketSender)
    {
        _socketReceiver = socketReceiver;
        _socketSender = socketSender;
    }

    public async Task<(CommandStatus status, TResp)> Execute<TResp>(CommunicationMessage message, CommunicationDefaultResponse<TResp> defaultResponse, Func<byte[], TResp> resultTranslation)
    {
        Logger.LogDebug($"Executing {message.Command.ToString()} command.");
        var sent = await SendAsync(_socketSender, message.Command, message.NewState, message.UserCode);
        if (!sent)
        {
            Logger.LogDebug("Command not send. Exiting.");
            return (CommandStatus.NotSent, defaultResponse.Value);
        }

        var (status, data) = await ReceiveAsync(_socketReceiver, defaultResponse.ExpectedCommand);
        Logger.LogDebug($"{message.Command.ToString()} executed with {status.ToString()} status.");
        return
            status != CommandStatus.Processed
                ? (status, defaultResponse.Value)
                : (CommandStatus.Processed, resultTranslation(data));
    }
}