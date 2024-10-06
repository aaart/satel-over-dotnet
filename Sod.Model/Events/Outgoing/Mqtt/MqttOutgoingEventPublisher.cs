using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Sod.Infrastructure.Capabilities;

namespace Sod.Model.Events.Outgoing.Mqtt;

public class MqttOutgoingEventPublisher : LoggingCapability, IOutgoingEventPublisher
{
    private readonly IMqttClient _client;
    private readonly OutgoingEventMappings _mappings;

    public MqttOutgoingEventPublisher(IMqttClient client, OutgoingEventMappings mappings)
    {
        _client = client;
        _mappings = mappings;
    }

    public async Task<IEnumerable<FailedOutgoingEvent>> PublishAsync(OutgoingEvent evnt)
    {
        var failed = new List<FailedOutgoingEvent>();
        var topics = _mappings.GetTopics(evnt);
        foreach (var topic in topics)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(evnt.Value)
                .Build();
            var publishResult = await _client.PublishAsync(msg);
            if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success) failed.Add(new FailedOutgoingEvent(evnt, FailedOutgoingEventReason.CommunicationError));
        }

        if (!topics.Any())
        {
            failed.Add(new FailedOutgoingEvent(evnt, FailedOutgoingEventReason.TopicNotFound));
            Logger.LogWarning($"Mapping for reference = {evnt.Reference} and type = {evnt.Type} not found.");
        }

        return failed;
    }
}