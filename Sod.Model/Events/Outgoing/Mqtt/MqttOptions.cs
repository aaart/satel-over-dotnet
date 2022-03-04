namespace Sod.Model.Events.Outgoing.Mqtt
{
    public class MqttOptions
    {
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public string User { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? CrtPath { get; init; } = null;
    }
}