using Sod.Infrastructure.Satel;

namespace Sod.Model.Events.Outgoing;

public record OutgoingEvent(OutgoingEventType Type, int Reference, string Value)
{
    public OutgoingEvent(OutgoingEventType type, int reference, bool value)
        : this(type, reference, OnOffParse.ToString(value))
    {
    }
}
