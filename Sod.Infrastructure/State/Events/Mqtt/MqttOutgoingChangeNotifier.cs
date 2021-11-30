using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet.Client.Publishing;

namespace Sod.Infrastructure.State.Events.Mqtt
{
    public class MqttOutgoingChangeNotifier : IOutgoingChangeNotifier
    {
        private readonly IMqttClient _mqttClient;
        private readonly OutgoingChangeMappings _mappings;

        public MqttOutgoingChangeNotifier(IMqttClient mqttClient, OutgoingChangeMappings mappings)
        {
            _mqttClient = mqttClient;
            _mappings = mappings;
        }
        
        public async Task<IEnumerable<FailedOutgoingChange>> Publish(OutgoingChange change)
        {
            var failed = new List<FailedOutgoingChange>();
            foreach (var topic in _mappings.GetTopics(change))
            {
                var publishResult = await _mqttClient.PublishAsync(topic, change.Value);
                if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    failed.Add(new FailedOutgoingChange(change));
                }
            }

            return failed;
        }
    }
}