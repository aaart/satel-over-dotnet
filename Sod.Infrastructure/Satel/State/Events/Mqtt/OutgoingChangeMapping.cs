namespace Sod.Infrastructure.Satel.State.Events.Mqtt
{
    public record OutgoingChangeMapping
    {
        public OutgoingChangeMapping(IOType type, int ioIndex, string topic)
        {
            Type = type;
            IOIndex = ioIndex;
            Topic = topic;
        }

        public IOType Type { get; private init; }
        public int IOIndex { get; private init; }
        public string Topic { get; private init; }
    }
}