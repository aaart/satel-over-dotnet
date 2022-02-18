using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Publishing;
using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.State.Events.Mqtt
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
        
        public async Task<IEnumerable<FailedOutgoingEvent>> PublishAsync(OutgoingEvent evnt)
        {
            var failed = new List<FailedOutgoingEvent>();
            foreach (var topic in _mappings.GetTopics(evnt))
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(evnt.Value)
                    .Build();
                var publishResult = await _publisher.PublishAsync(msg, CancellationToken.None);
                if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    failed.Add(new FailedOutgoingEvent(evnt));
                }
            }

            return failed;
        }
    }
}