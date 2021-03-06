using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Model.Events.Incoming.Events;

namespace Sod.Model.Events.Incoming
{
    public class Broker : LoggingCapability, IBroker
    {
        private readonly EventHandlerMappings _handlerMappings;

        public Broker(EventHandlerMappings handlerMappings)
        {
            _handlerMappings = handlerMappings;
        }

        public async Task Process(IncomingEvent incomingEvent)
        {
            Logger.LogDebug("an event received.");
            foreach (var handler in _handlerMappings.GetHandlers(incomingEvent.Topic))
            {
                await handler.HandleAsync(incomingEvent);
            }
        }
    }
}