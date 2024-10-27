using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;

namespace Sod.Model.Events.Incoming;

public class Broker : LoggingCapability, IBroker
{
    private readonly EventHandlerMappings _handlerMappings;

    public Broker(EventHandlerMappings handlerMappings)
    {
        _handlerMappings = handlerMappings;
    }

    public async Task Process(string topic, string payload)
    {
        Logger.LogInformation($"an event received. payload = {payload}; topic = {topic}");
        foreach (var handler in _handlerMappings.GetHandlers(topic)) await handler.HandleAsync(payload);
    }
}