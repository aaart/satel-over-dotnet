using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;

namespace Sod.Model.Events.Incoming
{
    public class Broker : LoggingCapability, IBroker
    {
        private readonly EventHandlerMappings _handlerMappings;

        public Broker(EventHandlerMappings handlerMappings)
        {
            _handlerMappings = handlerMappings;
        }

        public async Task Process(string topic, string payload)
        {
            Logger.LogDebug("an event received.");
            foreach (var handler in _handlerMappings.GetHandlers(topic))
            {
                await handler.HandleAsync(payload);
            }
        }
    }
}