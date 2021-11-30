namespace Sod.Infrastructure.State.Events.Mqtt
{
    public record OutgoingChangeMapping
    {
        public OutgoingChangeMapping(IOType type, int ioIndex, string topic)
        {
            Type = type;
            IOIndex = ioIndex;
            Topic = topic;
        }

        public IOType Type { get; init; }
        public int IOIndex { get; init; }
        public string Topic { get; init; }
    }
}