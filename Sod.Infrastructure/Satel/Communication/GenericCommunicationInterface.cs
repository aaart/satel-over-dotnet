using System;
using System.Threading;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Socket;
using static Sod.Infrastructure.Satel.Communication.Communication;

namespace Sod.Infrastructure.Satel.Communication
{
    public class GenericCommunicationInterface
    {
        private static readonly object _lock = new object();
        
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
            bool sent;
            try
            {
                Monitor.Enter(_lock);
                sent = await SendAsync(_socketSender, message.Command, message.NewState, message.UserCode);
            }
            finally
            {
                Monitor.Exit(_lock);
            }
            
            if (!sent)
            {
                return (CommandStatus.NotSent, defaultResponse.Value);
            }

            var (status, data) = await ReceiveAsync(_socketReceiver, defaultResponse.ExpectedCommand);
            return 
                status != CommandStatus.Processed 
                ? (status, defaultResponse.Value) 
                : (CommandStatus.Processed, resultTranslation(data));
        }
    }
}