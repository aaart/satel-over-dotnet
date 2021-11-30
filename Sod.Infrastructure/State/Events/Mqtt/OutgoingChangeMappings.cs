using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sod.Infrastructure.State.Events.Mqtt
{
    public class OutgoingChangeMappings : IEnumerable<OutgoingChangeMapping>
    {
        private readonly IEnumerable<OutgoingChangeMapping> _mappings;

        public OutgoingChangeMappings(IEnumerable<OutgoingChangeMapping> mappings)
        {
            _mappings = mappings;
        }

        public IEnumerator<OutgoingChangeMapping> GetEnumerator() => _mappings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public IEnumerable<string> GetTopics(OutgoingChange change)
        {
            IOType ioType;
            switch (change.Type)
            {
                case EventType.InputsStateChanged:
                    ioType = IOType.Input;
                    break;
                case EventType.OutputsStateChanged:
                    ioType = IOType.Output;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var mappings = _mappings.Where(x => x.Type == ioType && x.IOIndex == change.Reference).ToArray();
            return mappings.Select(x => x.Topic);
        }
    }
}