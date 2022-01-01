using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sod.Infrastructure.Satel.State.Events.Outgoing;

namespace Sod.Infrastructure.Satel.State.Events.Mqtt
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
            IOType ioType;
            switch (evnt.Type)
            {
                case OutgoingEventType.InputsStateChanged:
                    ioType = IOType.Input;
                    break;
                case OutgoingEventType.OutputsStateChanged:
                    ioType = IOType.Output;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _mappings
                .Where(x => x.Type == ioType && x.IOIndex == evnt.Reference)
                .Select(x => x.Topic);
        }
    }
}