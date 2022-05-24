namespace Sod.Model.Events.Outgoing.Mqtt
{
    public record OutgoingEventMapping
    {
        public DeviceType Type { get; init; }
        public int IOIndex { get; init; }
        public string Topic { get; init; } = string.Empty;
    }
}