namespace Sod.Infrastructure.Satel
{
    public enum CommandStatus
    { 
        Processed,
        NotSupportedCommand,
        InvalidFrame,
        InvalidCrc,
        InvalidCommandReceived,
        NotSent
    }
}