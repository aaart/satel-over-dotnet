using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sod.Model.CommonTypes;

namespace Sod.Model.Events.Outgoing.Mqtt
{
    public class OutgoingEventMappings : IEnumerable<OutgoingEventMapping>
    {
        private readonly IEnumerable<OutgoingEventMapping> _mappings;

        public OutgoingEventMappings(IEnumerable<OutgoingEventMapping> mappings)
        {
            _mappings = mappings;
        }

        public IEnumerator<OutgoingEventMapping> GetEnumerator() => _mappings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public IEnumerable<string> GetTopics(OutgoingEvent evnt)
        {
            DeviceType deviceType;
            switch (evnt.Type)
            {
                case OutgoingEventType.InputsStateChanged:
                    deviceType = DeviceType.Input;
                    break;
                case OutgoingEventType.OutputsStateChanged:
                    deviceType = DeviceType.Output;
                    break;
                case OutgoingEventType.ArmedPartitionsStateChanged:
                    deviceType = DeviceType.ArmedPartition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _mappings
                .Where(x => x.Type == deviceType && x.IOIndex == evnt.Reference)
                .Select(x => x.Topic);
        }
    }
}