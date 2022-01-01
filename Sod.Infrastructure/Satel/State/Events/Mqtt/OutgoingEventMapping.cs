namespace Sod.Infrastructure.Satel.State.Events.Mqtt
{
    public record OutgoingEventMapping
    {
        public IOType Type { get; init; }
        public int IOIndex { get; init; }
        public string Topic { get; init; } = string.Empty;
    }
}