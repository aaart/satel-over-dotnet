using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Publishing;
using Sod.Infrastructure.Satel.State.Events.Outgoing;

namespace Sod.Infrastructure.Satel.State.Events.Mqtt
{
    public class MqttOutgoingEventPublisher : IOutgoingEventPublisher
    {
        private readonly IApplicationMessagePublisher _publisher;
        private readonly OutgoingEventMappings _mappings;

        public MqttOutgoingEventPublisher(IApplicationMessagePublisher publisher, OutgoingEventMappings mappings)
        {
            _publisher = publisher;
            _mappings = mappings;
        }
        
        public async Task<IEnumerable<FailedOutgoingEvent>> Publish(OutgoingEvent change)
        {
            var failed = new List<FailedOutgoingEvent>();
            foreach (var topic in _mappings.GetTopics(change))
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(change.Value)
                    .Build();
                var publishResult = await _publisher.PublishAsync(msg, CancellationToken.None);
                if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    failed.Add(new FailedOutgoingEvent(change));
                }
            }

            return failed;
        }
    }
}