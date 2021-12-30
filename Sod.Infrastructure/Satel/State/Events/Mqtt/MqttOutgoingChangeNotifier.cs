using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Publishing;

namespace Sod.Infrastructure.Satel.State.Events.Mqtt
{
    public class MqttOutgoingChangeNotifier : IOutgoingChangeNotifier
    {
        private readonly IApplicationMessagePublisher _publisher;
        private readonly OutgoingChangeMappings _mappings;

        public MqttOutgoingChangeNotifier(IApplicationMessagePublisher publisher, OutgoingChangeMappings mappings)
        {
            _publisher = publisher;
            _mappings = mappings;
        }
        
        public async Task<IEnumerable<FailedOutgoingChange>> Publish(OutgoingChange change)
        {
            var failed = new List<FailedOutgoingChange>();
            foreach (var topic in _mappings.GetTopics(change))
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(change.Value)
                    .Build();
                var publishResult = await _publisher.PublishAsync(msg, CancellationToken.None);
                if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    failed.Add(new FailedOutgoingChange(change));
                }
            }

            return failed;
        }
    }
}