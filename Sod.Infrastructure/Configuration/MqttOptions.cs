namespace Sod.Infrastructure.Configuration
{
    public class MqttOptions
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? CrtPath { get; set; }
    }
}