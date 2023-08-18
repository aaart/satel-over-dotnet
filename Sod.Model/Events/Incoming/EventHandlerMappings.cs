using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sod.Model.Events.Incoming;

public class EventHandlerMappings : IEnumerable<IStateChangeDispatcher>
{
    private readonly Dictionary<string, IStateChangeDispatcher> _handlers = new();

    public EventHandlerMappings(IEnumerable<(string topic, IStateChangeDispatcher handler)> handlers)
    {
        foreach (var tuple in handlers) _handlers.Add(tuple.topic, tuple.handler);
    }

    public IEnumerable<string> Topics => _handlers.Keys;

    public IEnumerable<IStateChangeDispatcher> GetHandlers(string topic)
    {
        return _handlers.Where(x => string.Equals(x.Key, topic, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Value);
    }

    public IEnumerator<IStateChangeDispatcher> GetEnumerator()
    {
        return _handlers.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}