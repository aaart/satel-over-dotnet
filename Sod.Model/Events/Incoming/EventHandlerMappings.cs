using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sod.Model.Events.Incoming
{
    public class EventHandlerMappings : IEnumerable<IEventHandler>
    {
        private readonly Dictionary<string, IEventHandler> _handlers = new();

        public EventHandlerMappings(IEnumerable<(string topic, IEventHandler handler)> handlers)
        {
            foreach (var tuple in handlers)
            {
                _handlers.Add(tuple.topic, tuple.handler);
            }
        }

        public IEnumerable<string> Topics => _handlers.Keys;
        
        public IEnumerable<IEventHandler> GetHandlers(string topic) => _handlers.Where(x => string.Equals(x.Key, topic, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Value);

        public IEnumerator<IEventHandler> GetEnumerator() => _handlers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}